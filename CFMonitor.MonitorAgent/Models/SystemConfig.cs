using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace CFMonitor.Agent.Models
{
    public class SystemConfig
    {
        public string MonitorAgentId { get; set; } = String.Empty;

        public int LocalPort { get; set; }

        public string AgentManagerIp { get; set; } = String.Empty;

        public int AgentManagerPort { get; set; }
        
        public int MaxConcurrentChecks { get; set; }

        /// <summary>
        /// Security key for communication with Agent Manager
        /// </summary>
        public string SecurityKey { get; set; } = String.Empty;

        /// <summary>
        /// Root folder for monitor item files. E.g. Scripts to run
        /// </summary>
        public string MonitorItemFilesRootFolder { get; set; } = String.Empty;

        public int HeartbeatSecs { get; set; }

        public string LogFolder { get; set; } = String.Empty;

        public int MaxLogDays { get; set; }
    }
}
