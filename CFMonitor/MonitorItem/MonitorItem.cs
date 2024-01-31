using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    [XmlType("MonitorItem")]
    [XmlInclude(typeof(MonitorDHCP))]
    [XmlInclude(typeof(MonitorDiskSpace))]
    [XmlInclude(typeof(MonitorDNS))]
    [XmlInclude(typeof(MonitorFile))]
    [XmlInclude(typeof(MonitorJSON))]
    [XmlInclude(typeof(MonitorLDAP))]
    [XmlInclude(typeof(MonitorMemory))]
    [XmlInclude(typeof(MonitorPing))]
    [XmlInclude(typeof(MonitorProcess))]
    [XmlInclude(typeof(MonitorRegistry))]
    [XmlInclude(typeof(MonitorREST))]
    [XmlInclude(typeof(MonitorService))]
    [XmlInclude(typeof(MonitorSMTP))]
    [XmlInclude(typeof(MonitorSOAP))]
    [XmlInclude(typeof(MonitorSQL))]
    [XmlInclude(typeof(MonitorURL))]
    public class MonitorItem
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }
        [XmlElement("MonitorItemSchedule")]
        public MonitorItemSchedule MonitorItemSchedule = new MonitorItemSchedule();
        [XmlArray("EventItems")]
        [XmlArrayItem("EventItem")]
        public List<EventItem> EventItems = new List<EventItem>();
    }
}
