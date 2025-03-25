using CFConnectionMessaging.Models;
using CFMonitor.Agent.Models;
using CFMonitor.Constants;
using CFMonitor.Exceptions;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.Messages;
using CFMonitor.SystemTask;
using Microsoft.Extensions.DependencyInjection;
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

        private readonly IServiceProvider _serviceProvider;

        private List<ActiveMonitorItemTask> _activeMonitorItemTasks = new();

        private readonly SystemConfig _systemConfig;

        //private readonly ISystemTaskList _systemTaskList;

        // Whether we need to refresh local data
        private bool _isNeedToRefreshData = true;

        private List<MonitorItem> _monitorItems = new();

        // Local data services, populated from Agent Manager connection
        // We only need to store data used by IChecker
        private readonly IEventItemService _eventItemService;
        private readonly IMonitorAgentService _monitorAgentService;
        private readonly IMonitorItemService _monitorItemService;
        private readonly ISystemValueTypeService _systemValueTypeService;
             
        private class ActiveMonitorItemTask
        {
            public MonitorItem? MonitorItem { get; set; } = new();

            public Task<MonitorItemOutput>? Task { get; set; }
        }

        public Worker(IServiceProvider serviceProvider, SystemConfig systemConfig)
        {
            _serviceProvider = serviceProvider; 
            _systemConfig = systemConfig;
            //_systemTaskList = serviceProvider.GetRequiredService<ISystemTaskList>();

            _eventItemService = _serviceProvider.GetRequiredService<IEventItemService>();
            _monitorAgentService = _serviceProvider.GetRequiredService<IMonitorAgentService>();
            _monitorItemService = _serviceProvider.GetRequiredService<IMonitorItemService>();
            _systemValueTypeService = _serviceProvider.GetRequiredService<ISystemValueTypeService>();

            _timer = new System.Timers.Timer();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 5000;
            _timer.Enabled = false;

            // Set handler for monitor item updated message
            _managerConnection.OnMonitorItemUpdated += delegate (MonitorItemUpdated monitorItemUpdated)
            {
                Console.WriteLine("Received notification that monitor items have been updated");
                _isNeedToRefreshData = true;
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

                // Refresh data from Agent Manager
                if (_isNeedToRefreshData)
                {
                    GetMonitorAgents();
                    GetSystemValueTypes();
                    GetEventItems();
                    GetMonitorItems();

                    _isNeedToRefreshData = false;
                }

                // Only do processing if we have all data
                if (!_isNeedToRefreshData)
                {
                    UpdateHeartbeat(false);

                    CheckMonitorItems();

                    CheckMonitorItemsCompleted();
                }
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

        private EndpointInfo ManagerEndpointInfo => new EndpointInfo() { Ip = _systemConfig.AgentManagerIp, Port = _systemConfig.AgentManagerPort };

        /// <summary>
        /// Gets monitor agents from Agent Manager, saves to local storage
        /// </summary>
        private int GetMonitorAgents()
        {
            Console.WriteLine($"Getting Monitor Agents");

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
                _monitorAgentService.DeleteById(monitorAgents.First().Id);
                monitorAgents.RemoveAt(0);
            }

            // Save monitor agents
            foreach (var monitorAgent in response.MonitorAgents)
            {
                _monitorAgentService.Add(monitorAgent);
            }

            Console.WriteLine($"Got Monitor Agents");

            return response.MonitorAgents.Count;
        }

        /// <summary>
        /// Gets monitor item list from Agent Manager, saves to local storage
        /// </summary>
        private int GetMonitorItems()
        {
            Console.WriteLine($"Getting monitor items");

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
                _monitorItemService.DeleteById(monitorItems.First().Id);
                monitorItems.RemoveAt(0);
            }

            // Save monitor items
            foreach (var monitorItem in response.MonitorItems)
            {
                _monitorItemService.Add(monitorItem);
            }

            // Store monitor items
            _monitorItems = response.MonitorItems;

            Console.WriteLine($"Got monitor items");

            return response.MonitorItems.Count;
        }

        /// <summary>
        /// Gets event items from Agent Manager, saves to local storage
        /// </summary>
        private int GetEventItems()
        {
            Console.WriteLine($"Getting event items");

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
                    _eventItemService.DeleteById(eventItems.First().Id);
                    eventItems.RemoveAt(0);
                }

                // Save event items
                foreach (var eventItem in response.EventItems)
                {
                    _eventItemService.Add(eventItem);
                }
           
            Console.WriteLine($"Got event items");

            return response.EventItems.Count;
        }

        /// <summary>
        /// Gets system value types from Agent Manager, saves to local storage
        /// </summary>
        private int GetSystemValueTypes()
        {
            Console.WriteLine($"Getting system value types");

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
                _systemValueTypeService.DeleteById(systemValueTypes.First().Id);
                systemValueTypes.RemoveAt(0);
            }

            // Save system value types
            foreach (var systemValueType in response.SystemValueTypes)
            {
                _systemValueTypeService.Add(systemValueType);
            }

            Console.WriteLine($"Got system value types");

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

            // Check monitor items not active            
            foreach(var monitorItem in _monitorItems.Where(mi => !_activeMonitorItemTasks.Any(t => t.MonitorItem.Id == mi.Id)))
            {
                if (_activeMonitorItemTasks.Count < _systemConfig.MaxConcurrentChecks)
                {
                    if (monitorItem.MonitorItemSchedule.IsTime(DateTime.UtcNow))   // Overdue
                    {
                        monitorAgent = monitorAgent ?? _monitorAgentService.GetById(_systemConfig.MonitorAgentId);

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
                var monitorItemOutput = activeMonitorItemTask.Task.Result;

                // Set Monitor Agent Id as IChecker isn't aware of Monitor Agent
                monitorItemOutput.MonitorAgentId = _systemConfig.MonitorAgentId;

                var monitorItemResultMessage = new MonitorItemResultMessage()
                {                                        
                    SenderAgentId = _systemConfig.MonitorAgentId,
                    SecurityKey = _systemConfig.SecurityKey,
                    MonitorItemOutput = activeMonitorItemTask.Task.Result,
                };                

                _managerConnection.SendMonitorItemResultMessage(monitorItemResultMessage, null);  // TODO: Send endpoint
            }
            else    // Failed
            {

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
                Console.WriteLine($"Checking {monitorItem.Name}");

                var monitorItemOutput = new MonitorItemOutput();

                using (var scope = _serviceProvider.CreateScope())
                {
                    var checker = _serviceProvider.GetServices<IChecker>().FirstOrDefault(c => c.CanCheck(monitorItem));
                    if (checker != null)
                    {
                        monitorItemOutput = checker.CheckAsync(monitorAgent, monitorItem, checkerConfig).Result;
                    }
                }

                Console.WriteLine($"Checked {monitorItem.Name}");

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
                Console.WriteLine("Sending heartbeat");

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

                Console.WriteLine("Sent heartbeat");
            }  
        }
    }
}
