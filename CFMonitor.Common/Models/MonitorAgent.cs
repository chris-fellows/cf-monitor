using System;

namespace CFMonitor.Models
{
    /// <summary>
    /// Instance of monitor agent
    /// </summary>
    public class MonitorAgent
    {
        public string Id { get; set; } = String.Empty;
     
        /// <summary>
        /// Monitor item group
        /// </summary>
        public string MonitorAgentGroupId { get; set; } = String.Empty;

        /// <summary>
        /// Machine where agent running
        /// </summary>
        public string MachineName { get; set; } = String.Empty;

        /// <summary>
        /// Username of process
        /// </summary>
        public string UserName { get; set; } = String.Empty;

        /// <summary>
        /// IP for communications
        /// </summary>
        public string IP { get; set; } = String.Empty;

        /// <summary>
        /// Port for communications
        /// </summary>
        public int Port { get; set; }

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
