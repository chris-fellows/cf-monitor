using CFMonitor.AgentManager.Models;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.Messages;
using CFMonitor.SystemTask;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly AgentsConnection _agentConnection;   // = new AgentConnection()
        //private readonly ISystemTaskList _systemTaskList;
        
        public Worker(IServiceProvider serviceProvider,
                      SystemConfig systemConfig)
        {
            _serviceProvider = serviceProvider;
            _systemConfig = systemConfig;
            //_systemTaskList = serviceProvider.GetRequiredService<ISystemTaskList>();

            var auditEventFactory = _serviceProvider.GetRequiredService<IAuditEventFactory>();
            var auditEventService = _serviceProvider.GetRequiredService<IAuditEventService>();
            var eventItemService = _serviceProvider.GetRequiredService<IEventItemService>();
            var fileObjectService = _serviceProvider.GetRequiredService<IFileObjectService>();
            var monitorAgentService = _serviceProvider.GetRequiredService<IMonitorAgentService>();
            var monitorItemOutputService = _serviceProvider.GetRequiredService<IMonitorItemOutputService>();
            var monitorItemService = _serviceProvider.GetRequiredService<IMonitorItemService>();
            var userService = _serviceProvider.GetRequiredService<IUserService>();

            _agentConnection = new AgentsConnection(auditEventFactory, auditEventService, eventItemService, 
                            fileObjectService, monitorAgentService, monitorItemOutputService, 
                            monitorItemService, _serviceProvider, userService);

            _timer = new System.Timers.Timer();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 5000;
            _timer.Enabled = false;

            //// Set handler for get monitor items request
            //_agentConnection.OnGetMonitorItemsRequestReceived += delegate (GetMonitorItemsRequest request)
            //{
            //    var monitorItems = _monitorItemService.GetAll();

            //};

            //// Set handler for heartbeat received
            //_agentConnection.OnHeartbeatReceived += delegate (Heartbeat heartbeat)
            //{
            //    var monitorAgent = _monitorAgentService.GetById(heartbeat.SenderAgentId);
            //    if (monitorAgent != null)
            //    {
            //        monitorAgent.HeartbeatDateTime = DateTimeOffset.UtcNow;
            //        _monitorAgentService.Update(monitorAgent);
            //    }
            //};
        }

        public void Start()
        {
            _agentConnection.StartListening(_systemConfig.LocalPort);
        }

        public void Stop()
        {            
            _agentConnection.StopListening();
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timer.Enabled = false;
                
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
    }
}
