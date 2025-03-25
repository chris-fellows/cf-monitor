using CFMonitor.Enums;
using System.Xml.Serialization;

namespace CFMonitor.Models
{
    [XmlType("MonitorItem")] 
    public class MonitorItem
    {
        [XmlAttribute("Id")]
        public string Id { get; set; } = String.Empty;

        [XmlAttribute("Name")]
        public string Name { get; set; } = String.Empty;

        //[XmlAttribute("MonitorItemTypeId")]
        //public string MonitorItemTypeId { get; set; } = String.Empty;

        [XmlAttribute("MonitorItemType")]
        public MonitorItemTypes MonitorItemType { get; set; }

        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Monitor Agents that item should be checked on
        /// </summary>
        [XmlArray("MonitorAgentIds")]
        [XmlArrayItem("MonitorAgentId")]
        public List<string> MonitorAgentIds { get; set; } = new List<string>();

        /// <summary>
        /// Schedule for checking item
        /// </summary>
        [XmlElement("MonitorItemSchedule")]
        public MonitorItemSchedule MonitorItemSchedule { get; set; } = new MonitorItemSchedule();
                
        /// <summary>
        /// Parameters that are specific to monitor item type
        /// </summary>
        [XmlArray("Parameters")]
        [XmlArrayItem("Parameter")]
        public List<MonitorItemParameter> Parameters { get; set; } = new List<MonitorItemParameter>();           
    }
}
