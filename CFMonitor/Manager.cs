using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CFMonitor
{
    internal class Manager
    {
        private List<MonitorItem> _monitorItems = null;
        private System.Timers.Timer _timer;
        private Dictionary<MonitorItem, Task> _activeTasks = new Dictionary<MonitorItem, Task>();
        private const int _maxActiveTasks = 10;
        private List<IChecker> _checkerList = null;
        private List<IActioner> _actionerList = null;
        private IMonitorItemRepository _monitorItemRepository = null;

        public Manager(List<IChecker> checkerList, List<IActioner> actionerList, IMonitorItemRepository monitorItemRepository)
        {
            _checkerList = checkerList;
            _actionerList = actionerList;
            _monitorItemRepository = monitorItemRepository;
        }

        public void Start()
        {
            // Get all monitor items
            _monitorItems = _monitorItemRepository.GetAll();

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

        private static bool IsTaskComplete(Task task)
        {
            return (task.Status == TaskStatus.RanToCompletion || task.Status == TaskStatus.Faulted);
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timer.Enabled = false;
                CheckMonitorItems();
            }
            catch
            {

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
                int count = 0;
                foreach (MonitorItem monitorItem in _activeTasks.Keys)
                {              
                    if (!IsTaskComplete(_activeTasks[monitorItem]))
                    {
                        count++;
                    }
                }
                return count;
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
                if (monitorItem.MonitorItemSchedule.IsTime(DateTime.Now) && !_activeTasks.ContainsKey(monitorItem))
                {
                    if (ActiveTaskCount < _maxActiveTasks)
                    {
                        // Start task
                        Task task = CheckMonitorItem(monitorItem);
                        _activeTasks.Add(monitorItem, task);
                    }
                }
            }

            // Clean completed monitor items
            HandleCompletedMonitorItems();
        }        

        private void HandleCompletedMonitorItems()
        {
            List<MonitorItem> monitorItems = new List<MonitorItem>();

            // Get completed items
            foreach(MonitorItem monitorItem in _monitorItems)
            {
                if (_activeTasks.ContainsKey(monitorItem))
                {                  
                    if (IsTaskComplete(_activeTasks[monitorItem]))
                    {
                        monitorItems.Add(monitorItem);
                    }
                }
            }

            // Handle completion for each task
            while (monitorItems.Count > 0)
            {
                MonitorItem monitorItem = monitorItems[0];
                Task task = _activeTasks[monitorItem];
                HandleCompletedMonitorItem(monitorItem, task);
                _activeTasks.Remove(monitorItem);
                monitorItems.RemoveAt(0);
            }                
        }

        /// <summary>
        /// Handles completed task
        /// </summary>
        /// <param name="monitorItem"></param>
        /// <param name="task"></param>
        private void HandleCompletedMonitorItem(MonitorItem monitorItem, Task task)
        {

        }

        /// <summary>
        /// Starts task to check monitor item
        /// </summary>
        /// <param name="monitorItem"></param>
        /// <returns></returns>
        private Task CheckMonitorItem(MonitorItem monitorItem)
        {
            Task task = null;
            IChecker checker = _checkerList.Find(currentChecker => (currentChecker.CanCheck(monitorItem)));
            if (checker != null)
            {            
                task = Task.Factory.StartNew(() =>
                {
                    checker.Check(monitorItem, _actionerList);
                });                
            }
            return task;
        }    
    }
}
