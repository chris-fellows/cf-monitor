using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    [XmlType("MonitorPOP")]
    public class MonitorPOP : MonitorItem
    {
        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.POP;

        public override List<EventConditionSource> GetEventConditionSources()
        {
            return new List<EventConditionSource>()
            {
                EventConditionSource.Exception,
                EventConditionSource.NoException,
                EventConditionSource.POPConnected,
                EventConditionSource.POPConnectError
            };
        }
    }
}
