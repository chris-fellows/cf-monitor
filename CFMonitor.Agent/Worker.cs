using CFMonitor.Agent.Models;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.Messages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Agent
{
    /// <summary>
    /// Agent worker. Performs monitoring
    /// </summary>
    public class Worker
    {
        private readonly System.Timers.Timer _timer;
        
        private ManagerConnection _managerConnection = new ManagerConnection();

        private MonitorAgent? _monitorAgent;

        private List<MonitorItem> _monitorItems = new List<MonitorItem>();
        
        private DateTimeOffset _lastHeartbeatTime = DateTimeOffset.MinValue;

        private readonly IServiceProvider _serviceProvider;

        private List<ActiveMonitorItemTask> _activeMonitorItemTasks = new();

        private readonly SystemConfig _systemConfig;

        private class ActiveMonitorItemTask
        {
            public MonitorItem? MonitorItem { get; set; } = new();

            public Task<MonitorItemOutput>? Task { get; set; }
        }

        public Worker(IServiceProvider serviceProvider, SystemConfig systemConfig)
        {
            _serviceProvider = serviceProvider; 
            _systemConfig = systemConfig;
            
            _timer = new System.Timers.Timer();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 5000;
            _timer.Enabled = false;

            // Set handler for monitor item updated message
            _managerConnection.OnMonitorItemUpdated += delegate (MonitorItemUpdated monitorItemUpdated)
            {
                _monitorItems.Clear();
            };
        }

        public void Start()
        {
            _timer.Enabled = true;

            _managerConnection.StartListening(_systemConfig.LocalPort);          
        }

        public void Stop()
        {
            _timer.Enabled = false;

            _managerConnection.StopListening();
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timer.Enabled = false;

                // Get monitor items from Agent Manager
                if (!_monitorItems.Any())
                {
                    GetMonitorItems();
                }
                
                UpdateHeartbeat(false);

                CheckMonitorItems();
                
                CheckMonitorItemsCompleted();
            }
            catch
            {
                // TODO: Log error
            }
            finally
            {
                _timer.Enabled = true;
            }
        }

        /// <summary>
        /// Gets monitor item list from Agent Manager
        /// </summary>
        private void GetMonitorItems()
        {            
            // Send request, wait for response
            var request = new GetMonitorItemsRequest() { Id = Guid.NewGuid().ToString() };
            var response = _managerConnection.SendGetMonitorItems(request, null);

            // Store monitor items
            _monitorItems = response.MonitorItems;           
        }

        /// <summary>
        /// Checks overdue monitor items
        /// </summary>
        private void CheckMonitorItems()
        {
            // Check monitor items not active            
            foreach(var monitorItem in _monitorItems.Where(mi => !_activeMonitorItemTasks.Any(t => t.MonitorItem.Id == mi.Id)))
            {
                if (_activeMonitorItemTasks.Count < _systemConfig.MaxConcurrentChecks)
                {
                    if (monitorItem.MonitorItemSchedule.IsTime(DateTime.UtcNow))   // Overdue
                    {
                        var activeTask = new ActiveMonitorItemTask()
                        {
                            MonitorItem = monitorItem,
                            Task = CheckMonitorItemAsync(_monitorAgent, monitorItem)
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

                ProcessCompletedMonitorItem(task);
            }
        }

        /// <summary>
        /// Processes completed monitor item task
        /// </summary>
        /// <param name="activeMonitorItemTask"></param>
        private void ProcessCompletedMonitorItem(ActiveMonitorItemTask activeMonitorItemTask)
        {
            if (activeMonitorItemTask.Task.Exception != null)
            {

            }
            else
            {

            }
        }

        /// <summary>
        /// Checks monitor item
        /// </summary>
        /// <param name="monitorAgent"></param>
        /// <param name="monitorItem"></param>
        /// <returns></returns>
        private Task<MonitorItemOutput> CheckMonitorItemAsync(MonitorAgent monitorAgent, MonitorItem monitorItem)
        {
            return Task.Factory.StartNew(() =>
            {
                var monitorItemOutput = new MonitorItemOutput();

                using (var scope = _serviceProvider.CreateScope())
                {
                    var checker = _serviceProvider.GetServices<IChecker>().FirstOrDefault(c => c.CanCheck(monitorItem));
                    if (checker != null)
                    {
                        monitorItemOutput = checker.CheckAsync(monitorAgent, monitorItem, false).Result;
                    }
                }
                
                return monitorItemOutput;
            });
        }

        /// <summary>
        /// Updates monitor agent heartbeat
        /// </summary>        
        private void UpdateHeartbeat(bool force)
        {
            if (force || _lastHeartbeatTime.AddSeconds(60) <= DateTimeOffset.UtcNow)
            {
                _lastHeartbeatTime = DateTimeOffset.UtcNow;

                var heartbeat = new Heartbeat() { Id = Guid.NewGuid().ToString() };
                _managerConnection.SendHeartbeat(heartbeat, null);
            }  
        }
    }
}
