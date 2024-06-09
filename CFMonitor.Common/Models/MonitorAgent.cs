using System;

namespace CFMonitor.Models
{
    /// <summary>
    /// Instance of monitor agent
    /// </summary>
    public class MonitorAgent
    {
        public string ID { get; set; } = String.Empty;

        public string MachineName { get; set; } = String.Empty;

        public string UserName { get; set; } = String.Empty;

        public DateTimeOffset HeartbeatDateTime { get; set; } = DateTimeOffset.MinValue;
    }
}
