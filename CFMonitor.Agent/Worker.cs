using CFMonitor.Models;
using CFMonitor.Models.Messages;
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

        private readonly int _port;
        private ManagerConnection _managerConnection = new ManagerConnection();

        private List<MonitorItem> _monitorItems = new List<MonitorItem>();
        
        private DateTimeOffset _lastHeartbeatTime = DateTimeOffset.MinValue;

        public Worker(int port)
        {
            _port = port;
                
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

            _managerConnection.StartListening(_port);            
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

                // Heartneat
                UpdateHeartbeat(false);

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
            foreach(var monitorItem in _monitorItems)
            {
                
            }
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
