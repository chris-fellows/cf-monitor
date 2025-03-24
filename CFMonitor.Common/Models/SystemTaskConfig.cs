namespace CFMonitor.Models
{
    /// <summary>
    /// System task config
    /// </summary>
    public class SystemTaskConfig
    {
        /// <summary>
        /// System task name
        /// </summary>
        public string SystemTaskName { get; set; } = String.Empty;

        /// <summary>
        /// Execute frequency
        /// </summary>
        public TimeSpan ExecuteFrequency { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Next regular execute time
        /// </summary>
        public DateTimeOffset NextExecuteTime { get; set; }
    }
}
