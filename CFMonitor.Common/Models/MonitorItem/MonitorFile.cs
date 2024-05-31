using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    /// <summary>
    /// Settings for monitoring a file
    /// </summary>
    [XmlType("MonitorFile")]
    public class MonitorFile : MonitorItem
    {
        [XmlAttribute("FileName")]
        public string FileName { get; set; }
        [XmlAttribute("FindText")]
        public string FindText { get; set; }       // Optional text to find

        public override List<EventConditionSource> GetEventConditionSources()
        {
            return new List<EventConditionSource>()
            {
                EventConditionSource.Exception,
                EventConditionSource.NoException,
                EventConditionSource.FileExists,
                EventConditionSource.FileNotExists,
                EventConditionSource.TextFoundInFile,
                EventConditionSource.TextNotFoundInFile
            };
        }
    }
}
