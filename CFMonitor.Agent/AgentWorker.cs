using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Agent
{
    public class AgentWorker
    {
        private readonly System.Timers.Timer _timer;

        public AgentWorker()
        {
            _timer = new System.Timers.Timer();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 5000;
            _timer.Enabled = false;
        }

        public void Start()
        {
            _timer.Enabled = true;
        }

        public void Stop()
        {
            _timer.Enabled = false;
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timer.Enabled = false;

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

        private void CheckMonitorItems()
        {

        }

        /// <summary>
        /// Updates monitor agent heartbeat
        /// </summary>
        /// <param name="force"></param>
        private void UpdateHeartbeat(bool force)
        {
            //if (force || _lastHeartbeat.Add(_monitorAgentConfig.HeartbeatInterval) <= DateTimeOffset.UtcNow)
            //{
            //    _lastHeartbeat = DateTimeOffset.UtcNow;

            //    // Get monitor agent instance
            //    var monitorAgent = _monitorAgentService.GetByFilter((agent) =>
            //    {
            //        return agent.MachineName == Environment.MachineName &&
            //                    agent.UserName == Environment.UserName;
            //    }).FirstOrDefault();

            //    if (monitorAgent == null)     // New instance
            //    {
            //        monitorAgent = new MonitorAgent()
            //        {
            //            ID = Guid.NewGuid().ToString(),
            //            MachineName = Environment.MachineName,
            //            UserName = Environment.UserName,
            //            HeartbeatDateTime = DateTime.UtcNow
            //        };
            //        _monitorAgentService.Insert(monitorAgent);
            //    }
            //    else    // Existing instance
            //    {
            //        monitorAgent.HeartbeatDateTime = DateTime.UtcNow;
            //        _monitorAgentService.Update(monitorAgent);
            //    }
            //}
        }
    }
}
