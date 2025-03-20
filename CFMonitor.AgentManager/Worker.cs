using CFMonitor.Interfaces;
using CFMonitor.Models.Messages;
using CFMonitor.Models.MonitorItems;
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

        private readonly int _port;
        private AgentConnection _agentConnection = new AgentConnection();

        private readonly IMonitorItemService _monitorItemService;
        private readonly IMonitorAgentService _monitorAgentService;

        public Worker(int port)
        {
            _port = port;

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
            _agentConnection.StartListening(_port);
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
