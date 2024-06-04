using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    /// <summary>
    /// Settings for monitoring a process by starting it and waiting for it to complete. E.g. PowerShell script.
    /// </summary>
    [XmlType("MonitorRunProcess")]
    public class MonitorRunProcess : MonitorItem
    {
        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.RunProcess;

        [XmlAttribute("FileName")]
        public string FileName { get; set; }

        public override List<EventConditionSource> GetEventConditionSources()
        {
            return new List<EventConditionSource>()
            {
                EventConditionSource.Exception,
                EventConditionSource.NoException,
                EventConditionSource.RunProcessExitCodeReturned                
            };
        }
    }
}
