using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    [XmlType("MonitorItem")]
    [XmlInclude(typeof(MonitorDHCP))]
    [XmlInclude(typeof(MonitorDiskSpace))]
    [XmlInclude(typeof(MonitorDNS))]
    [XmlInclude(typeof(MonitorLocalFile))]
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
    public abstract class MonitorItem
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

        public virtual List<EventConditionSource> GetEventConditionSources()
        {
            return new List<EventConditionSource>();
        }

        /// <summary>
        /// Monitor item type. It links to the MonitorItemType instance so that we can get other details such
        /// as which IChecker to use
        /// </summary>
        public virtual MonitorItemTypes MonitorItemType { get; }
    }
}
