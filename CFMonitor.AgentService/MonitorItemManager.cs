using CFMonitor.AgentService.Models;
using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CFMonitor.AgentService
{
    /// <summary>
    /// Manages monitor items
    /// </summary>
    internal class MonitorItemManager
    {
        private List<MonitorItem> _monitorItems = new List<MonitorItem>();
        private System.Timers.Timer _timer;
        private List<MonitorItemTaskInfo> _taskInfos = new List<MonitorItemTaskInfo>();        
        private readonly MonitorAgentConfig _monitorAgentConfig;
        private readonly IActionersService _actionersService;
        private readonly ICheckersService _checkersService;
        private readonly IMonitorAgentService _monitorAgentService;
        private readonly IMonitorItemService _monitorItemService;
        private readonly IMonitorItemTypeService _monitorItemTypeService;
        private DateTimeOffset _lastRefreshMonitorItems = DateTimeOffset.MinValue;
        private DateTimeOffset _lastHeartbeat = DateTimeOffset.MinValue;

        public MonitorItemManager(IActionersService actionersService, 
                                ICheckersService checkersService,
                                IMonitorAgentService monitorAgentService,
                                IMonitorItemService monitorItemService,
                                IMonitorItemTypeService monitorItemTypeService, 
                                MonitorAgentConfig monitorAgentConfig)
        {
            _actionersService = actionersService;
            _checkersService = checkersService;
            _monitorAgentService = monitorAgentService;
            _monitorItemService = monitorItemService;
            _monitorItemTypeService = monitorItemTypeService;
            _monitorAgentConfig = monitorAgentConfig;
        }

        public void Start()
        {
            RefreshMonitorItems(true);

            if (_timer == null)
            {
                _timer = new System.Timers.Timer();
                _timer.Elapsed += _timer_Elapsed;
                _timer.Interval = 5000;
            }
            _timer.Start();                       
        }

        /// <summary>
        /// Refreshes monitor item list if forced or overdue.
        /// 
        /// Consider executing a 'Refresh' command on each service when updated by CFMonitor.Manager
        /// </summary>
        /// <param name="force"></param>
        private void RefreshMonitorItems(bool force)
        {
            if (force || _lastRefreshMonitorItems.AddMinutes(5) <= DateTimeOffset.UtcNow)
            {
                _lastRefreshMonitorItems = DateTimeOffset.UtcNow;

                // Store old items
                var oldMonitorItems = new List<MonitorItem>();
                oldMonitorItems.AddRange(_monitorItems);
                _monitorItems.Clear();

                _monitorItems = _monitorItemService.GetAll();
                foreach (var monitorItem in _monitorItems)
                {
                    // Check if previously loaded
                    var oldMonitorItem = oldMonitorItems.FirstOrDefault(mi => mi.ID == monitorItem.ID);

                    // Set schedule last checked
                    if (oldMonitorItem == null)   // Not already initialised
                    {
                        if (monitorItem.MonitorItemSchedule.ScheduleType == ScheduleTypes.FixedInterval)        // Every N units
                        {
                            // TODO: Consider ensuring that this is always at offsets from 00:00 so that item is checked at predicatable
                            // intervals. E.g. If interval is every 10 mins then set minutes to be 0, 10, 20, 30 etc.
                            monitorItem.MonitorItemSchedule.TimeLastChecked = DateTime.Now;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates monitor agent heartbeat
        /// </summary>
        /// <param name="force"></param>
        private void UpdateHeartbeat(bool force)
        {
            if (force || _lastHeartbeat.Add(_monitorAgentConfig.HeartbeatInterval) <= DateTimeOffset.UtcNow)
            {
                _lastHeartbeat = DateTimeOffset.UtcNow;

                // Get monitor agent instance
                var monitorAgent = _monitorAgentService.GetByFilter((agent) =>
                {
                    return agent.MachineName == Environment.MachineName &&
                                agent.UserName == Environment.UserName;
                }).FirstOrDefault();

                if (monitorAgent == null)     // New instance
                {
                    monitorAgent = new MonitorAgent()
                    {
                        ID = Guid.NewGuid().ToString(),
                        MachineName = Environment.MachineName,
                        UserName = Environment.UserName,
                        HeartbeatDateTime= DateTime.UtcNow
                    };
                    _monitorAgentService.Insert(monitorAgent);
                }
                else    // Existing instance
                {
                    monitorAgent.HeartbeatDateTime = DateTime.UtcNow;
                    _monitorAgentService.Update(monitorAgent);
                }
            }
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timer.Enabled = false;

                UpdateHeartbeat(false);

                // Periodic refresh of monitor items
                RefreshMonitorItems(false);

                // Start monitor items
                CheckMonitorItems();
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
    
        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }     

        private int ActiveTaskCount
        {
            get
            {
                return _taskInfos.Count(t => !t.Task.IsCompleted);
            }
        }

        /// <summary>
        /// Checks monitor items:
        /// - Starts check if time overdue.
        /// - Checks completed items.
        /// </summary>
        private void CheckMonitorItems()
        {
            // Clean completed monitor items
            HandleCompletedMonitorItems();                                    

            // Start overdue tasks
            foreach(MonitorItem monitorItem in _monitorItems)
            {
                if (monitorItem.MonitorItemSchedule.IsTime(DateTime.Now) && 
                    !_taskInfos.Any(t => t.MonitorItem.ID ==  monitorItem.ID))
                {
                    if (ActiveTaskCount < _monitorAgentConfig.MaxConcurrentMonitorItems)
                    {
                        // Create tasks
                        var taskInfo = new MonitorItemTaskInfo()
                        {
                            MonitorItem = monitorItem,
                            TimeStarted = DateTime.Now,
                            Task = CheckMonitorItemAsync(monitorItem)
                        };                                                
                        _taskInfos.Add(taskInfo);
                    }
                }
            }

            // Clean completed monitor items
            HandleCompletedMonitorItems();
        }        

        /// <summary>
        /// Handles completed monitor items
        /// </summary>
        private void HandleCompletedMonitorItems()
        {
            var completedTaskInfos = _taskInfos.Where(ti => ti.Task.IsCompleted).ToList();
            
            // Handle completion for each task
            while (completedTaskInfos.Count > 0)
            {
                var taskInfo = completedTaskInfos.First();
                completedTaskInfos.Remove(taskInfo);
                
                HandleCompletedMonitorItem(taskInfo.MonitorItem, taskInfo.Task);                
            }                
        }

        /// <summary>
        /// Handles completed monitor item
        /// </summary>
        /// <param name="monitorItem"></param>
        /// <param name="task"></param>
        private void HandleCompletedMonitorItem(MonitorItem monitorItem, Task task)
        {
            // TODO: Handle completed task
        }

        /// <summary>
        /// Starts task to check monitor item
        /// </summary>
        /// <param name="monitorItem"></param>
        /// <returns></returns>
        private Task CheckMonitorItemAsync(MonitorItem monitorItem)
        {                        
            var monitorItemType = _monitorItemTypeService.GetAll().First(mit => mit.ItemType == monitorItem.MonitorItemType);

            var actioners = _actionersService.GetAll();            

            // Check checker
            IChecker checker = _checkersService.GetAll().First(c => c.CheckerType == monitorItemType.CheckerType);
                            
            // Check
            var task = checker.CheckAsync(monitorItem, actioners);                
            
            return task;
        }    
    }
}
