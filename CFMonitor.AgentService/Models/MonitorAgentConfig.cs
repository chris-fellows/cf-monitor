using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.AgentService.Models
{
    public class MonitorAgentConfig
    {
        /// <summary>
        /// Max number of concurrent monitor items that can be checked
        /// </summary>
        public int MaxConcurrentMonitorItems { get; set; }

        /// <summary>
        /// Frequency that agent service updates heartbeat
        /// </summary>
        public TimeSpan HeartbeatInterval { get; set; }
    }
}
