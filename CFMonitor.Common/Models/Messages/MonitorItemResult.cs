namespace CFMonitor.Models.Messages
{
    /// <summary>
    /// Result of checking monitor item
    /// </summary>
    public class MonitorItemResult : MessageBase
    {
        /// <summary>
        /// Monitor item checked
        /// </summary>
        public string MonitorItemId { get; set; } = String.Empty;

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
