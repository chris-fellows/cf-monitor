using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    /// <summary>
    /// Settings for monitoring folder size
    /// </summary>
    [XmlType("MonitorFolderSize")]
    public class MonitorFolderSize : MonitorItem
    {
        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.FolderSize;

        [XmlAttribute("Folder")]
        public string Folder { get; set; }

        /// <summary>
        /// Max folder size (bytes) to be inside tolerance
        /// </summary>
        [XmlAttribute("MaxFolderSizeBytes")]
        public double MaxFolderSizeBytes { get; set; }

        public override List<EventConditionSource> GetEventConditionSources()
        {
            return new List<EventConditionSource>()
            {
                EventConditionSource.Exception,
                EventConditionSource.NoException,
                EventConditionSource.FolderSizeInTolerance,
                EventConditionSource.FolderSizeOutsideTolerance
            };
        }
    }
}
