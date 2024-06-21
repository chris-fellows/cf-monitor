using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    /// <summary>
    /// Settings for monitoring a local file. Can optionally check file content for particular text.
    /// </summary>
    [XmlType("MonitorFile")]
    public class MonitorLocalFile : MonitorItem
    {
        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.LocalFile;

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
