using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    [XmlType("MonitorSMTP")]
    public class MonitorSMTP : MonitorItem
    {
        [XmlAttribute("Server")]
        public string Server { get; set; }
        [XmlAttribute("Port")]
        public int Port { get; set; }

        public override List<EventConditionSource> GetEventConditionSources()
        {
            return new List<EventConditionSource>()
            {
                EventConditionSource.Exception,
                EventConditionSource.NoException                
            };
        }
    }
}
