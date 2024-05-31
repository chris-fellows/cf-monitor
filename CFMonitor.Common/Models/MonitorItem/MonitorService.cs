using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    /// <summary>
    /// Settings for monitoring a service
    /// </summary>
    [XmlType("MonitorService")]
    public class MonitorService : MonitorItem
    {
        [XmlAttribute("MachineName")]
        public string MachineName { get; set; }
        [XmlAttribute("ServiceName")]
        public string ServiceName { get; set; }

        public override List<EventConditionSource> GetEventConditionSources()
        {
            return new List<EventConditionSource>()
            {
                EventConditionSource.Exception,
                EventConditionSource.NoException,
                EventConditionSource.ServiceControllerStatus                
            };
        }
    }
}
