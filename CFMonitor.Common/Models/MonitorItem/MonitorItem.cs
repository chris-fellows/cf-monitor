using CFMonitor.Common.Models.MonitorItem;
using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    [XmlType("MonitorItem")]
    //[XmlInclude(typeof(MonitorActiveProcess))]
    //[XmlInclude(typeof(MonitorCPU))]
    //[XmlInclude(typeof(MonitorDHCP))]
    //[XmlInclude(typeof(MonitorDiskSpace))]
    //[XmlInclude(typeof(MonitorDNS))]
    //[XmlInclude(typeof(MonitorFileSize))]
    //[XmlInclude(typeof(MonitorFolderSize))]
    //[XmlInclude(typeof(MonitorIMAP))]
    //[XmlInclude(typeof(MonitorLocalFile))]
    //[XmlInclude(typeof(MonitorJSON))]
    //[XmlInclude(typeof(MonitorLDAP))]
    //[XmlInclude(typeof(MonitorMemory))]
    //[XmlInclude(typeof(MonitorNTP))]
    //[XmlInclude(typeof(MonitorPing))]
    //[XmlInclude(typeof(MonitorPOP))]
    //[XmlInclude(typeof(MonitorRegistry))]
    //[XmlInclude(typeof(MonitorREST))]
    //[XmlInclude(typeof(MonitorRunProcess))]
    //[XmlInclude(typeof(MonitorService))]
    //[XmlInclude(typeof(MonitorSMTP))]
    //[XmlInclude(typeof(MonitorSOAP))]
    //[XmlInclude(typeof(MonitorSQL))]
    //[XmlInclude(typeof(MonitorURL))]
    public class MonitorItem
    {
        [XmlAttribute("ID")]
        public string ID { get; set; } = String.Empty;
        [XmlAttribute("Name")]
        public string Name { get; set; } = String.Empty;

        [XmlAttribute("MonitorItemTypes")]
        public MonitorItemTypes MonitorItemType { get; set; }

        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }

        [XmlElement("MonitorItemSchedule")]
        public MonitorItemSchedule MonitorItemSchedule = new MonitorItemSchedule();
        
        [XmlArray("EventItems")]
        [XmlArrayItem("EventItem")]
        public List<EventItem> EventItems = new List<EventItem>();

        [XmlArray("Parameters")]
        [XmlArrayItem("Parameter")]
        public List<MonitorItemParameter> Parameters = new List<MonitorItemParameter>();

        public virtual List<EventConditionSource> GetEventConditionSources()
        {
            return new List<EventConditionSource>();
        }               
    }
}
