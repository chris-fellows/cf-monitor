using CFMonitor.Enums;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    /// <summary>
    /// Settings for monitoring active process
    /// </summary>
    [XmlType("MonitorActiveProcess")]
    public class MonitorActiveProcess : MonitorItem
    {
        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.ActiveProcess;

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
                EventConditionSource.ActiveProcessRunning,
                EventConditionSource.ActiveProcessNotRunning
            };
        }
    }
}
