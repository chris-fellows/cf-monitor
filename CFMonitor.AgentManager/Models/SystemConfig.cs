using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.AgentManager.Models
{
    public class SystemConfig
    {
        public string MonitorAgentManagerId { get; set; } = String.Empty;
                
        public int LocalPort { get; set; }

        public int MaxConcurrentMessages { get; set; }

        public int HeartbeatSecs { get; set; }
        public string LogFolder { get; set; } = String.Empty;

        public int MaxLogDays { get; set; }

    }
}
