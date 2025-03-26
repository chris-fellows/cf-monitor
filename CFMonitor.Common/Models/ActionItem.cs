using CFMonitor.Enums;
using System.Xml.Serialization;

namespace CFMonitor.Models
{
    /// <summary>
    /// Details of action to take when monitor item event has happened
    /// </summary>
    [XmlType("ActionItem")]   
    public class ActionItem
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public string Id { get; set; } = String.Empty;

        public ActionerTypes ActionerType { get; set; }

        public List<ActionItemParameter> Parameters { get; set; } = new();
    }
}
