namespace CFMonitor.Agent.Models
{
    public class SystemConfig
    {
        public int LocalPort { get; set; }
        
        public int MaxConcurrentChecks { get; set; }

        /// <summary>
        /// Security key for communication with Agent Manager
        /// </summary>
        public string SecurityKey { get; set; } = String.Empty;

        /// <summary>
        /// Root folder for monitor item files. E.g. Scripts to run
        /// </summary>
        public string MonitorItemFilesRootFolder { get; set; } = String.Empty;
    }
}
