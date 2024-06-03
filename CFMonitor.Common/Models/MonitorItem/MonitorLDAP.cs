using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    [XmlType("MonitorLDAP")]
    public class MonitorLDAP : MonitorItem
    {
        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.LDAP;

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
