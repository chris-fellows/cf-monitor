﻿using CFMonitor.Enums;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    [XmlType("MonitorItem")] 
    public class MonitorItem
    {
        [XmlAttribute("Id")]
        public string Id { get; set; } = String.Empty;

        [XmlAttribute("Name")]
        public string Name { get; set; } = String.Empty;

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
        public MonitorItemSchedule MonitorItemSchedule = new MonitorItemSchedule();
        
        /// <summary>
        /// Events to handle
        /// </summary>
        [XmlArray("EventItems")]
        [XmlArrayItem("EventItem")]
        public List<EventItem> EventItems = new List<EventItem>();

        /// <summary>
        /// Parameters that are specific to monitor item type
        /// </summary>
        [XmlArray("Parameters")]
        [XmlArrayItem("Parameter")]
        public List<MonitorItemParameter> Parameters = new List<MonitorItemParameter>();           
    }
}
