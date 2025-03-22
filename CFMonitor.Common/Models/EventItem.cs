using System.Xml.Serialization;

namespace CFMonitor.Models
{
    /// <summary>
    /// Details of event to handle for specific monitor item being checked. Defines condition for event and
    /// action(s) to take.
    /// </summary>
    [XmlType("EventItem")]
    public class EventItem
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Monitor item that it relates to
        /// </summary>
        public string MonitorItemId { get; set; } = String.Empty;

        [XmlElement("EventCondition")]
        public EventCondition EventCondition = new EventCondition();  
        
        [XmlArray("ActionItems")]
        [XmlArrayItem("ActionItem")]
        public List<ActionItem> ActionItems = new List<ActionItem>();
    }
}
