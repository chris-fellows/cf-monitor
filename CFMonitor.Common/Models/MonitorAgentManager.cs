using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Models
{
    public class MonitorAgentManager
    {
        public string Id { get; set; } = String.Empty;
        
        /// <summary>
        /// Machine where agent running
        /// </summary>
        public string MachineName { get; set; } = String.Empty;

        /// <summary>
        /// Username of process
        /// </summary>
        public string UserName { get; set; } = String.Empty;

        /// <summary>
        /// Version of Monitor Agent software
        /// </summary>
        public string Version { get; set; } = String.Empty;      

        /// <summary>
        /// Last heartbeat
        /// </summary>
        public DateTimeOffset HeartbeatDateTime { get; set; } = DateTimeOffset.MinValue;
    }
}
