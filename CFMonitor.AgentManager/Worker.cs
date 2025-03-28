using CFConnectionMessaging.Models;
using CFMonitor.AgentManager.Models;
using CFMonitor.Interfaces;
using CFMonitor.Log;
using CFMonitor.Models;
using CFMonitor.Models.Messages;
using CFMonitor.SystemTask;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.AgentManager
{
    /// <summary>
    /// Agent Manager worker
    /// </summary>
    internal class Worker
    {
        private readonly System.Timers.Timer _timer;

        private readonly IServiceProvider _serviceProvider;
        private readonly SystemConfig _systemConfig;
        private readonly AgentsConnection _agentConnection;                                                           

        private readonly IMonitorAgentManagerService _monitorAgentManagerService;

        private DateTimeOffset _lastHeartbeatTime = DateTimeOffset.MinValue;
        private DateTimeOffset _lastArchiveLogsTime = DateTimeOffset.MinValue;

        private readonly ISimpleLog _log;

        private ConcurrentQueue<QueueItem> _queueItems = new();

        private class QueueItemTask
        {
            public Task Task { get; internal set; }

            public QueueItem QueueItem { get; internal set; }

            public QueueItemTask(Task task, QueueItem queueItem)
            {
                Task = task;
                QueueItem = queueItem;
            }
        }

        private List<QueueItemTask> _queueItemTasks = new List<QueueItemTask>();

        public Worker(IServiceProvider serviceProvider,
                      SystemConfig systemConfig)
        {
            _serviceProvider = serviceProvider;
            _systemConfig = systemConfig;

            var auditEventFactory = _serviceProvider.GetRequiredService<IAuditEventFactory>();
            var auditEventService = _serviceProvider.GetRequiredService<IAuditEventService>();
            var eventItemService = _serviceProvider.GetRequiredService<IEventItemService>();
            var fileObjectService = _serviceProvider.GetRequiredService<IFileObjectService>();
            var monitorAgentService = _serviceProvider.GetRequiredService<IMonitorAgentService>();
            var monitorItemOutputService = _serviceProvider.GetRequiredService<IMonitorItemOutputService>();
            var monitorItemService = _serviceProvider.GetRequiredService<IMonitorItemService>();
            var userService = _serviceProvider.GetRequiredService<IUserService>();

            _log = _serviceProvider.GetRequiredService<ISimpleLog>();

            _monitorAgentManagerService = _serviceProvider.GetRequiredService<IMonitorAgentManagerService>();

            _agentConnection = new AgentsConnection(auditEventFactory, auditEventService, eventItemService, 
                            fileObjectService, _log, monitorAgentService, monitorItemOutputService, 
                            monitorItemService, _serviceProvider, userService);

            _agentConnection.OnConnectionMessageReceived += delegate (ConnectionMessage connectionMessage, MessageReceivedInfo messageReceivedInfo)
            {
                var queueItem = new QueueItem()
                {
                    ConnectionMessage = connectionMessage,
                    MessageReceivedInfo = messageReceivedInfo
                };
                _queueItems.Enqueue(queueItem);

                if (_timer.Interval == 5000) _timer.Interval = 100;                
            };

            _timer = new System.Timers.Timer();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 5000;
            _timer.Enabled = false;       
        }

        public void Start()
        {
            _log.Log(DateTimeOffset.UtcNow, "Information", "Worker starting");

            _timer.Enabled = true;

            _agentConnection.StartListening(_systemConfig.LocalPort);            
        }

        public void Stop()
        {
            _log.Log(DateTimeOffset.UtcNow, "Information", "Worker stopping");

            _timer.Enabled = false;

            _agentConnection.StopListening();            
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timer.Enabled = false;

                UpdateHeartbeat(false);

                CheckCompleteQueueItemTasks(_queueItemTasks);

                // Process queue
                while (_queueItems.Any())
                {
                    if (_queueItemTasks.Count < _systemConfig.MaxConcurrentMessages)
                    {
                        if (_queueItems.TryDequeue(out QueueItem queueItem))
                        {
                            if (queueItem.ConnectionMessage != null)
                            {
                                var queueItemTask = new QueueItemTask(_agentConnection.HandleConnectionMessageAsync(queueItem.ConnectionMessage, queueItem.MessageReceivedInfo), queueItem);
                                _queueItemTasks.Add(queueItemTask);
                            }
                        }
                    }
                    else    // Max messages processed
                    {
                        Thread.Sleep(100);
                    }

                    UpdateHeartbeat(false);

                    CheckCompleteQueueItemTasks(_queueItemTasks);
                }

                // Archive logs
                if (_lastArchiveLogsTime.AddHours(12) <= DateTimeOffset.UtcNow)
                {
                    ArchiveLogs();
                }

                CheckCompleteQueueItemTasks(_queueItemTasks);
            }
            catch(Exception exception)
            {
                _log.Log(DateTimeOffset.UtcNow, "Error", $"Error performed regular functions: {exception.Message}");
            }
            finally
            {
                _timer.Interval = _queueItems.Any() ||
                                   _queueItemTasks.Any() ? 100 : 5000;
                _timer.Enabled = true;
            }
        }

        private void CheckCompleteQueueItemTasks(List<QueueItemTask> queueItemTasks)
        {
            // Get completed tasks
            var completedTasks = queueItemTasks.Where(t => t.Task.IsCompleted).ToList();

            // Process completed tasks
            while (completedTasks.Any())
            {
                var queueItemTask = completedTasks.First();
                completedTasks.Remove(queueItemTask);
                queueItemTasks.Remove(queueItemTask);

                ProcessCompletedQueueItemTask(queueItemTask);
            }
        }

        private void ProcessCompletedQueueItemTask(QueueItemTask queueItemTask)
        {

        }

        /// <summary>
        /// Updates monitor agent manager heartbeat
        /// </summary>        
        private void UpdateHeartbeat(bool force)
        {
            if (force || _lastHeartbeatTime.AddSeconds(_systemConfig.HeartbeatSecs) <= DateTimeOffset.UtcNow)
            {                
                _lastHeartbeatTime = DateTimeOffset.UtcNow;

                var monitorAgentManager = _monitorAgentManagerService.GetByIdAsync(_systemConfig.MonitorAgentManagerId).Result;
                if (monitorAgentManager == null)    // First time
                {
                    monitorAgentManager = new MonitorAgentManager()
                    {
                        Id = _systemConfig.MonitorAgentManagerId,
                        HeartbeatDateTime  = DateTimeOffset.UtcNow,
                        MachineName = Environment.MachineName,
                        UserName = Environment.UserName,
                        Version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
                    };

                    _monitorAgentManagerService.AddAsync(monitorAgentManager).Wait();
                }
                else       // New
                {
                    monitorAgentManager.HeartbeatDateTime = DateTimeOffset.UtcNow;
                    monitorAgentManager.MachineName = Environment.MachineName;
                    monitorAgentManager.UserName = Environment.UserName;
                    monitorAgentManager.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                    _monitorAgentManagerService.AddAsync(monitorAgentManager).Wait();
                }                
            }
        }

        /// <summary>
        /// Archives logs
        /// </summary>
        private void ArchiveLogs()
        {
            DateTimeOffset date = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(_systemConfig.MaxLogDays));

            _lastArchiveLogsTime = DateTimeOffset.UtcNow;

            for (int index = 0; index < 30; index++)
            {
                var logFile = Path.Combine(_systemConfig.LogFolder, $"MonitorAgentManager-{date.Subtract(TimeSpan.FromDays(index)).ToString("yyyy-MM-dd")}.txt");
                if (File.Exists(logFile))
                {
                    File.Delete(logFile);
                }
            }
        }
    }
}
