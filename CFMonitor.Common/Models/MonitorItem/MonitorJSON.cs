using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    [XmlType("MonitorJSON")]
    public class MonitorJSON : MonitorItem
    {
        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.JSON;

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
