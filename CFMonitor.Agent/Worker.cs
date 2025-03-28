﻿using CFConnectionMessaging.Models;
using CFMonitor.Agent.Models;
using CFMonitor.Common.Interfaces;
using CFMonitor.Constants;
using CFMonitor.Enums;
using CFMonitor.Exceptions;
using CFMonitor.Interfaces;
using CFMonitor.Log;
using CFMonitor.Models;
using CFMonitor.Models.Messages;
using CFMonitor.SystemTask;
using CFUtilities.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;

namespace CFMonitor.Agent
{
    /// <summary>
    /// Agent worker. Performs monitoring.
    /// 
    /// Notes:
    /// - Data is obtained from Agent Manager and stored locally.
    /// - Items and checked and result is sent to Agent Manager.
    /// </summary>
    public class Worker
    {
        private readonly System.Timers.Timer _timer;

        private ManagerConnection _managerConnection = new ManagerConnection();

        private DateTimeOffset _lastHeartbeatTime = DateTimeOffset.MinValue;
        private DateTimeOffset _lastArchiveLogsTime = DateTimeOffset.MinValue;
        //private DateTimeOffset _lastSendLogsTime = DateTimeOffset.MinValue;

        private readonly IServiceProvider _serviceProvider;

        private List<ActiveMonitorItemTask> _activeMonitorItemTasks = new();

        private readonly SystemConfig _systemConfig;

        //private readonly ISystemTaskList _systemTaskList;

        // Whether we need to refresh local data
        private bool _isNeedToRefreshServerData = true;

        private List<MonitorItem> _monitorItems = new();

        // Local data services, populated from Agent Manager connection
        // We only need to store data used by IChecker
        private readonly IEventItemService _eventItemService;
        private readonly IFileObjectService _fileObjectService;
        private readonly IMonitorAgentService _monitorAgentService;
        private readonly IMonitorItemCheckService _monitorItemCheckService;
        private readonly IMonitorItemService _monitorItemService;
        private readonly INameValueItemService _nameValueItemService;
        private readonly ISystemValueTypeService _systemValueTypeService;

        private readonly ISimpleLog _log;

        private ConcurrentQueue<QueueItem> _queueItems = new();

        private class ActiveMonitorItemTask
        {
            public MonitorItem? MonitorItem { get; set; } = new();

            public Task<MonitorItemOutput>? Task { get; set; }
        }

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


        public Worker(IServiceProvider serviceProvider, SystemConfig systemConfig)
        {
            _serviceProvider = serviceProvider;
            _systemConfig = systemConfig;
            //_systemTaskList = serviceProvider.GetRequiredService<ISystemTaskList>();

            _eventItemService = _serviceProvider.GetRequiredService<IEventItemService>();
            _fileObjectService = _serviceProvider.GetRequiredService<IFileObjectService>();
            _monitorAgentService = _serviceProvider.GetRequiredService<IMonitorAgentService>();
            _monitorItemCheckService = _serviceProvider.GetRequiredService<IMonitorItemCheckService>();
            _monitorItemService = _serviceProvider.GetRequiredService<IMonitorItemService>();
            _nameValueItemService = _serviceProvider.GetRequiredService<INameValueItemService>();
            _systemValueTypeService = _serviceProvider.GetRequiredService<ISystemValueTypeService>();

            _log = _serviceProvider.GetRequiredService<ISimpleLog>();

            _timer = new System.Timers.Timer();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 5000;
            _timer.Enabled = false;           

            _managerConnection.OnConnectionMessageReceived += delegate (ConnectionMessage connectionMessage, MessageReceivedInfo messageReceivedInfo)
            {
                var queueItem = new QueueItem()
                {
                    ConnectionMessage = connectionMessage,
                    MessageReceivedInfo = messageReceivedInfo
                };
                _queueItems.Enqueue(queueItem);

                if (_timer.Interval == 5000) _timer.Interval = 100;
            };
        }

        public void Start()
        {
            _log.Log(DateTimeOffset.UtcNow, "Information", "Worker starting");

            _timer.Enabled = true;

            _managerConnection.StartListening(_systemConfig.LocalPort);
        }

        public void Stop()
        {
            _log.Log(DateTimeOffset.UtcNow, "Information", "Worker stopping");

            _timer.Enabled = false;

            _managerConnection.StopListening();
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timer.Enabled = false;

                // Refresh data from Agent Manager
                if (_isNeedToRefreshServerData)
                {
                    RefreshServerData();
                }

                // Process queue items
                while (_queueItems.Any())
                {                    
                    if (_queueItems.TryDequeue(out QueueItem queueItem))
                    {
                        if (queueItem.ConnectionMessage != null)
                        {
                            //var queueItemTask = new QueueItemTask(_agentConnection.HandleConnectionMessageAsync(queueItem.ConnectionMessage, queueItem.MessageReceivedInfo), queueItem);
                            //_queueItemTasks.Add(queueItemTask);
                        }
                    }

                    CheckCompleteQueueItemTasks(_queueItemTasks);
                }

                CheckCompleteQueueItemTasks(_queueItemTasks);

                // Only do processing if we have all data
                if (!_isNeedToRefreshServerData)
                {
                    UpdateHeartbeat(false);

                    CheckMonitorItems();

                    CheckMonitorItemsCompleted();
                }

                // Archive logs
                if (_lastArchiveLogsTime.AddHours(12) <= DateTimeOffset.UtcNow)
                {
                    ArchiveLogs();
                }
            }
            catch (Exception exception)
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

        /// <summary>
        /// Refreshes server data from Agent Manager
        /// </summary>
        private void RefreshServerData()
        {
            _log.Log(DateTimeOffset.UtcNow, "Information", "Refreshing data from Agent Manager");

            GetMonitorAgents();
            GetSystemValueTypes();
            GetEventItems();
            GetMonitorItems();
            GetFileObjects();   // Must be done after get system value types & monitor items

            _isNeedToRefreshServerData = false;

            _log.Log(DateTimeOffset.UtcNow, "Information", "Refreshed data from Agent Manager");
        }

        private EndpointInfo ManagerEndpointInfo => new EndpointInfo() { Ip = _systemConfig.AgentManagerIp, Port = _systemConfig.AgentManagerPort };

        private List<SystemValueType> GetSystemValueTypesForFileObjects()
        {
            var systemValueTypes = _systemValueTypeService.GetAll();

            return systemValueTypes.Where(svt => (new[] { SystemValueTypes.MIP_RunProcessFileObjectId }).Contains(svt.ValueType)).ToList();
        }

        /// <summary>
        /// Gets file objects from Agent Manager, saves to local storage. We get file objects for monitor event parameters        
        /// </summary>
        private int GetFileObjects()
        {
            _log.Log(DateTimeOffset.UtcNow, "Information", $"Getting file objects");

            // Delete old monitor items
            var fileObjects = _fileObjectService.GetAll();
            while (fileObjects.Any())
            {
                _fileObjectService.DeleteByIdAsync(fileObjects.First().Id).Wait();
                fileObjects.RemoveAt(0);
            }

            // Get system value types that refer to file objects
            var systemValueTypes = GetSystemValueTypesForFileObjects();

            // Get file object Ids for parameters that refer to file objects
            var fileObjectIds = _monitorItems.Where(mi => mi.MonitorAgentIds.Contains(_systemConfig.MonitorAgentId))
                                        .SelectMany(mi => mi.Parameters)
                                        .Where(p => !String.IsNullOrEmpty(p.Value) && systemValueTypes.Select(svt => svt.Id).Contains(p.SystemValueTypeId))
                                        .Select(p => p.Value).Distinct().ToList();

            foreach (var fileObjectId in fileObjectIds)
            {
                _log.Log(DateTimeOffset.UtcNow, "Information", $"Getting file object {fileObjectId}");

                // Send request, wait for response
                var request = new GetFileObjectRequest()
                {
                    SenderAgentId = _systemConfig.MonitorAgentId,
                    SecurityKey = _systemConfig.SecurityKey,
                    FileObjectId = fileObjectId
                };
                var response = _managerConnection.SendGetFileObject(request, ManagerEndpointInfo);

                // Save file object
                if (response.FileObject != null)
                {
                    _fileObjectService.AddAsync(response.FileObject).Wait();
                }

                _log.Log(DateTimeOffset.UtcNow, "Information", $"Got file object {fileObjectId}");
            }

            _log.Log(DateTimeOffset.UtcNow, "Information", $"Got file objects");

            return fileObjectIds.Count;
        }

        /// <summary>
        /// Gets monitor agents from Agent Manager, saves to local storage
        /// </summary>
        private int GetMonitorAgents()
        {
            _log.Log(DateTimeOffset.UtcNow, "Information", $"Getting Monitor Agents");

            // Send request, wait for response
            var request = new GetMonitorAgentsRequest()
            {
                SenderAgentId = _systemConfig.MonitorAgentId,
                SecurityKey = _systemConfig.SecurityKey
            };
            var response = _managerConnection.SendGetMonitorAgents(request, ManagerEndpointInfo);

            // Delete old monitor items
            var monitorAgents = _monitorAgentService.GetAll();
            while (monitorAgents.Any())
            {
                _monitorAgentService.DeleteByIdAsync(monitorAgents.First().Id).Wait();
                monitorAgents.RemoveAt(0);
            }

            // Save monitor agents
            foreach (var monitorAgent in response.MonitorAgents)
            {
                _monitorAgentService.AddAsync(monitorAgent).Wait();
            }

            _log.Log(DateTimeOffset.UtcNow, "Information", $"Got Monitor Agents");

            return response.MonitorAgents.Count;
        }

        /// <summary>
        /// Gets monitor item list from Agent Manager, saves to local storage
        /// </summary>
        private int GetMonitorItems()
        {
            _log.Log(DateTimeOffset.UtcNow, "Information", $"Getting monitor items");

            // Send request, wait for response
            var request = new GetMonitorItemsRequest()
            {
                SenderAgentId = _systemConfig.MonitorAgentId,
                SecurityKey = _systemConfig.SecurityKey
            };
            var response = _managerConnection.SendGetMonitorItems(request, ManagerEndpointInfo);

            // Delete old monitor items
            var monitorItems = _monitorItemService.GetAll();
            while (monitorItems.Any())
            {
                _monitorItemService.DeleteByIdAsync(monitorItems.First().Id).Wait();
                monitorItems.RemoveAt(0);
            }

            // Save monitor items
            foreach (var monitorItem in response.MonitorItems)
            {
                _monitorItemService.AddAsync(monitorItem).Wait();
            }

            // Store monitor items
            _monitorItems = response.MonitorItems;

            _log.Log(DateTimeOffset.UtcNow, "Information", $"Got monitor items");

            return response.MonitorItems.Count;
        }

        /// <summary>
        /// Gets event items from Agent Manager, saves to local storage
        /// </summary>
        private int GetEventItems()
        {
            _log.Log(DateTimeOffset.UtcNow, "Information", $"Getting event items");

            // Send request, wait for response
            var request = new GetEventItemsRequest()
            {
                SenderAgentId = _systemConfig.MonitorAgentId,
                SecurityKey = _systemConfig.SecurityKey
            };

            var response = _managerConnection.SendGetEventItems(request, ManagerEndpointInfo);

            // Delete old monitor items
            var eventItems = _eventItemService.GetAll();
            while (eventItems.Any())
            {
                _eventItemService.DeleteByIdAsync(eventItems.First().Id).Wait();
                eventItems.RemoveAt(0);
            }

            // Save event items
            foreach (var eventItem in response.EventItems)
            {
                _eventItemService.AddAsync(eventItem).Wait();
            }

            _log.Log(DateTimeOffset.UtcNow, "Information", $"Got event items");

            return response.EventItems.Count;
        }

        /// <summary>
        /// Gets system value types from Agent Manager, saves to local storage
        /// </summary>
        private int GetSystemValueTypes()
        {
            _log.Log(DateTimeOffset.UtcNow, "Information", $"Getting system value types");

            // Send request, wait for response
            var request = new GetSystemValueTypesRequest()
            {
                SenderAgentId = _systemConfig.MonitorAgentId,
                SecurityKey = _systemConfig.SecurityKey
            };
            var response = _managerConnection.SendGetSystemValueTypes(request, ManagerEndpointInfo);

            // Delete old monitor items
            var systemValueTypes = _systemValueTypeService.GetAll();
            while (systemValueTypes.Any())
            {
                _systemValueTypeService.DeleteByIdAsync(systemValueTypes.First().Id).Wait();
                systemValueTypes.RemoveAt(0);
            }

            // Save system value types
            foreach (var systemValueType in response.SystemValueTypes)
            {
                _systemValueTypeService.AddAsync(systemValueType).Wait();
            }

            _log.Log(DateTimeOffset.UtcNow, "Information", $"Got system value types");

            return response.SystemValueTypes.Count;
        }

        /// <summary>
        /// Checks overdue monitor items
        /// </summary>
        private void CheckMonitorItems()
        {
            // Set checker config
            var checkerConfig = new CheckerConfig()
            {
                TestMode = false,
                FilesRootFolder = _systemConfig.MonitorItemFilesRootFolder
            };

            MonitorAgent? monitorAgent = null;

            // Check monitor items for this Monitor Agent and not active            
            foreach (var monitorItem in _monitorItems.Where(mi => mi.MonitorAgentIds.Contains(_systemConfig.MonitorAgentId) &&
                                                    !_activeMonitorItemTasks.Any(t => t.MonitorItem.Id == mi.Id)))
            {
                if (_activeMonitorItemTasks.Count < _systemConfig.MaxConcurrentChecks)
                {
                    var now = DateTime.UtcNow;

                    // Get time last checked
                    var monitorItemCheck = _monitorItemCheckService.GetByIdAsync(monitorItem.Id).Result;
                    if (monitorItemCheck == null)   // First time checking item
                    {
                        monitorItemCheck = new MonitorItemCheck()
                        {
                            Id = monitorItem.Id,
                            TimeLastChecked = DateTimeUtilities.GetStartOfDay(DateTimeOffset.UtcNow).DateTime   // Times always offset from start of day
                        };
                        _monitorItemCheckService.AddAsync(monitorItemCheck).Wait();
                    }

                    // Check if time, returns new TimeLastChecked value if item must be checked
                    var timeLastChecked = monitorItem.MonitorItemSchedule.CheckIsTime(DateTime.UtcNow, monitorItemCheck.TimeLastChecked);
                    if (timeLastChecked != null)        // Need to check item
                    {
                        monitorAgent = monitorAgent ?? _monitorAgentService.GetByIdAsync(_systemConfig.MonitorAgentId).Result;

                        // Update time last checked
                        monitorItemCheck.TimeLastChecked = timeLastChecked.Value;
                        _monitorItemCheckService.UpdateAsync(monitorItemCheck).Wait();

                        var activeTask = new ActiveMonitorItemTask()
                        {
                            MonitorItem = monitorItem,
                            Task = CheckMonitorItemAsync(monitorAgent, monitorItem, checkerConfig)
                        };
                        _activeMonitorItemTasks.Add(activeTask);
                    }
                }
            }
        }

        /// <summary>
        /// Checks monitor item checks completed
        /// </summary>
        private void CheckMonitorItemsCompleted()
        {
            // Get completed tasks
            var completedTasks = _activeMonitorItemTasks.Where(t => t.Task != null && t.Task.IsCompleted).ToList();

            // Process completed tasks
            while (completedTasks.Any())
            {
                var task = completedTasks.First();
                completedTasks.Remove(task);
                _activeMonitorItemTasks.Remove(task);

                ProcessCompletedMonitorItem(task);
            }
        }

        /// <summary>
        /// Processes completed monitor item task
        /// </summary>
        /// <param name="activeMonitorItemTask"></param>
        private void ProcessCompletedMonitorItem(ActiveMonitorItemTask activeMonitorItemTask)
        {
            if (activeMonitorItemTask.Task.Exception == null)       // Success
            {
                _log.Log(DateTimeOffset.UtcNow, "Information", $"Sending monitor item output for {activeMonitorItemTask.MonitorItem.Name} to Agent Manager");

                var monitorItemOutput = activeMonitorItemTask.Task.Result;

                var monitorItemResultMessage = new MonitorItemResultMessage()
                {
                    SenderAgentId = _systemConfig.MonitorAgentId,
                    SecurityKey = _systemConfig.SecurityKey,
                    MonitorItemOutput = monitorItemOutput
                };

                _managerConnection.SendMonitorItemResultMessage(monitorItemResultMessage, ManagerEndpointInfo);

                _log.Log(DateTimeOffset.UtcNow, "Information", $"Sent monitor item output for {activeMonitorItemTask.MonitorItem.Name} to Agent Manager");
            }
            else    // Failed
            {
                _log.Log(DateTimeOffset.UtcNow, "Error", $"Failed checking monitor item {activeMonitorItemTask.MonitorItem.Name}: {activeMonitorItemTask.Task.Exception.Message}");
            }
        }

        /// <summary>
        /// Checks monitor item
        /// </summary>
        /// <param name="monitorAgent"></param>
        /// <param name="monitorItem"></param>
        /// <returns></returns>
        private Task<MonitorItemOutput> CheckMonitorItemAsync(MonitorAgent monitorAgent, MonitorItem monitorItem, CheckerConfig checkerConfig)
        {
            return Task.Factory.StartNew(() =>
            {
                _log.Log(DateTimeOffset.UtcNow, "Information", $"Checking {monitorItem.Name}");

                var monitorItemOutput = new MonitorItemOutput();

                using (var scope = _serviceProvider.CreateScope())
                {
                    var checker = _serviceProvider.GetServices<IChecker>().FirstOrDefault(c => c.CanCheck(monitorItem));
                    if (checker != null)
                    {
                        monitorItemOutput = checker.CheckAsync(monitorAgent, monitorItem, checkerConfig).Result;
                    }
                }

                _log.Log(DateTimeOffset.UtcNow, "Information", $"Checked {monitorItem.Name}");

                return monitorItemOutput;
            });
        }

        /// <summary>
        /// Updates monitor agent heartbeat
        /// 
        /// Note: We should only send the heartbeat if we're healthy so that Agent Manager knows that there's an issue
        /// </summary>        
        private void UpdateHeartbeat(bool force)
        {
            if (force || _lastHeartbeatTime.AddSeconds(_systemConfig.HeartbeatSecs) <= DateTimeOffset.UtcNow)
            {
                _lastHeartbeatTime = DateTimeOffset.UtcNow;

                var heartbeat = new Heartbeat()
                {
                    SenderAgentId = _systemConfig.MonitorAgentId,
                    SecurityKey = _systemConfig.SecurityKey,
                    MachineName = Environment.MachineName,
                    UserName = Environment.UserName,
                    Version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
                };
                _managerConnection.SendHeartbeat(heartbeat, ManagerEndpointInfo);
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
                var logFile = Path.Combine(_systemConfig.LogFolder, $"MonitorAgent-{date.Subtract(TimeSpan.FromDays(index)).ToString("yyyy-MM-dd")}.txt");
                if (File.Exists(logFile))
                {
                    File.Delete(logFile);
                }
            }
        }

        ///// <summary>
        ///// Sends log
        ///// </summary>
        //private void SendLogs()
        //{
        //    _lastSendLogsTime = DateTimeOffset.UtcNow;

        //    var monitorAgentLogMessage = new MonitorAgentLogMessage()
        //    {
        //        SenderAgentId = _systemConfig.MonitorAgentId,
        //        SecurityKey  = _systemConfig.SecurityKey,                 
        //    };
        //} 


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
    }
}
