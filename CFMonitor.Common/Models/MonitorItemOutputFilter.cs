namespace CFMonitor.Models
{
    /// <summary>
    /// Filter for MonitorItemOutput instances
    /// </summary>
    public class MonitorItemOutputFilter
    {
        public List<string>? MonitorAgentIds { get; set; }

        public List<string>? MonitorItemIds { get; set; }

        public DateTimeOffset? CheckedDateTimeFrom { get; set; }

        public DateTimeOffset? CheckedDateTimeTo { get; set; }

        /// <summary>
        /// Whether to only return the latest for each Monitor Agent
        /// </summary>
        public bool LatestOnly { get; set; }
    }
}
