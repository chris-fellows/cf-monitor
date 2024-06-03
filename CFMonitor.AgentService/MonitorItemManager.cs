using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CFMonitor.AgentService
{
    /// <summary>
    /// Manages monitor items
    /// </summary>
    internal class MonitorItemManager
    {
        private List<MonitorItem> _monitorItems = null;
        private System.Timers.Timer _timer;
        private List<MonitorItemTaskInfo> _taskInfos = new List<MonitorItemTaskInfo>();
        private const int _maxActiveTasks = 10;
        private readonly IActionersService _actionersService;
        private readonly ICheckersService _checkersService;
        private readonly IMonitorItemService _monitorItemService;
        private readonly IMonitorItemTypeService _monitorItemTypeService;

        public MonitorItemManager(IActionersService actionersService, 
                                ICheckersService checkersService,
                                IMonitorItemService monitorItemService,
                                IMonitorItemTypeService monitorItemTypeService)
        {
            _actionersService = actionersService;
            _checkersService = checkersService;
            _monitorItemService = monitorItemService;
            _monitorItemTypeService = monitorItemTypeService;
        }

        public void Start()
        {
            // Get all monitor items
            _monitorItems = _monitorItemService.GetAll();

            // Reset all items to start from now
            foreach (MonitorItem monitorItem in _monitorItems)
            {
                if (monitorItem.MonitorItemSchedule.ScheduleType == ScheduleTypes.FixedInterval)        // Every N units
                {
                    monitorItem.MonitorItemSchedule.TimeLastChecked = DateTime.Now;
                }
            }
           
            if (_timer == null)
            {
                _timer = new System.Timers.Timer();
                _timer.Elapsed += _timer_Elapsed;
                _timer.Interval = 5000;
            }
            _timer.Start();                       
        }

        //private static bool IsTaskComplete(Task task)
        //{
        //    return (task.Status == TaskStatus.RanToCompletion || task.Status == TaskStatus.Faulted);
        //}

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timer.Enabled = false;
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
                    if (ActiveTaskCount < _maxActiveTasks)
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
