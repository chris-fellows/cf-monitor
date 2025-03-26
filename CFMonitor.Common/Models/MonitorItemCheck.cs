using System.Xml.Serialization;

namespace CFMonitor.Models
{
    /// <summary>
    /// Monitor item check details. It only exists on the local Monitor Agent.
    /// </summary>
    public class MonitorItemCheck
    {
        /// <summary>
        /// Monitor Item Id
        /// </summary>
        [XmlAttribute("Id")]
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Time that item was last checked
        /// </summary>
        [XmlAttribute("TimeLastChecked")]
        public DateTime TimeLastChecked { get; set; }
    }
}
