using CFMonitor.Enums;
using CFMonitor.Models.MonitorItems;

namespace CFMonitor.Models
{
    /// <summary>
    /// Monitor item type
    /// </summary>
    public class MonitorItemType
    {
        /// <summary>
        /// Name for display
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Monitor item type
        /// </summary>
        public MonitorItemTypes ItemType { get; set; }

        /// <summary>
        /// Event conditions that are relevant
        /// </summary>
        public List<EventConditionSources> EventConditionSources { get; set; } = new List<EventConditionSources>();      

        /// <summary>
        /// Checker type
        /// </summary>
        public CheckerTypes CheckerType { get; set; }

        public List<MonitorItemParameter> DefaultParameters = new List<MonitorItemParameter>();
    }
}
