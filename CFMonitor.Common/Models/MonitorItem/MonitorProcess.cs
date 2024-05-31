using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    /// <summary>
    /// Settings for monitoring a process
    /// </summary>
    [XmlType("MonitorProcess")]
    public class MonitorProcess : MonitorItem
    {
        [XmlAttribute("MachineName")]
        public string MachineName { get; set; }
        [XmlAttribute("FileName")]
        public string FileName { get; set; }

        public override List<EventConditionSource> GetEventConditionSources()
        {
            return new List<EventConditionSource>()
            {
                EventConditionSource.Exception,
                EventConditionSource.NoException,
                EventConditionSource.ProcessRunning,
                EventConditionSource.ProcessNotRunning
            };
        }
    }
}
