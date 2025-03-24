namespace CFMonitor.Models
{
    /// <summary>
    /// Monitor item output
    /// </summary>
    public class MonitorItemOutput
    {
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Monitor item
        /// </summary>
        public string MonitorItemId { get; set; } = String.Empty;

        /// <summary>
        /// Monitor agent that checked monitor item
        /// </summary>
        public string MonitorAgentId { get; set; } = String.Empty;
        
        /// <summary>
        /// Time checked
        /// </summary>
        public DateTimeOffset CheckedDateTime { get; set; }

        /// <summary>
        /// Event items that need action
        /// </summary>
        public List<string> EventItemIdsForAction { get; set; } = new();

        /// <summary>
        /// Parameters for actions
        /// </summary>
        public List<ActionItemParameter> ActionItemParameters { get; set; } = new();
    }
}
