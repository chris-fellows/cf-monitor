using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    [XmlType("MonitorDNS")]
    public class MonitorDNS : MonitorItem
    {
        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.DNS;

        [XmlAttribute("Host")]
        public string Host { get; set; }

        public override List<EventConditionSource> GetEventConditionSources()
        {
            return new List<EventConditionSource>()
            {
                EventConditionSource.Exception,
                EventConditionSource.NoException,
                EventConditionSource.HostEntryExists,
                EventConditionSource.HostEntryNotExists
            };
        }
    }
}
