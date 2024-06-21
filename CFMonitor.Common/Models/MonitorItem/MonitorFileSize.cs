using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    /// <summary>
    /// Settings for monitoring file size
    /// </summary>
    [XmlType("MonitorFileSize")]
    public class MonitorFileSize : MonitorItem
    {
        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.FileSize;

        [XmlAttribute("File")]
        public string File { get; set; }

        /// <summary>
        /// Max folder size (bytes) to be inside tolerance
        /// </summary>
        [XmlAttribute("MaxFileSizeBytes")]
        public double MaxFileSizeBytes { get; set; }

        public override List<EventConditionSource> GetEventConditionSources()
        {
            return new List<EventConditionSource>()
            {
                EventConditionSource.Exception,
                EventConditionSource.NoException,
                EventConditionSource.FileSizeInTolerance,
                EventConditionSource.FileSizeOutsideTolerance
            };
        }
    }
}
