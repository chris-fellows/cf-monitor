using CFMonitor.Enums;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

namespace CFMonitor.Models.MonitorItems
{
    [XmlType("MonitorDHCP")]
    public class MonitorDHCP : MonitorItem
    {
        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.DHCP;

        [XmlAttribute("Server")]
        public string Server { get; set; }        

        public override List<EventConditionSource> GetEventConditionSources()
        {
            return new List<EventConditionSource>()
            {
                EventConditionSource.Exception,
                EventConditionSource.NoException,
                //EventConditionSource.HostEntryExists,
                //EventConditionSource.HostEntryNotExists
            };
        }
    }
}
