using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    [XmlType("MonitorREST")]
    public class MonitorREST : MonitorItem
    {
        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.REST;

        [XmlAttribute("URL")]
        public string URL { get; set; }

        public override List<EventConditionSource> GetEventConditionSources()
        {
            return new List<EventConditionSource>()
            {
                EventConditionSource.Exception,
                EventConditionSource.NoException,
                EventConditionSource.HttpResponseStatusCode,
                EventConditionSource.WebExceptionStatus
            };
        }
    }
}
