//using CFMonitor.Enums;
//using System;
//using System.Collections.Generic;
//using System.Xml.Serialization;

//namespace CFMonitor.Models.MonitorItems
//{
//    [XmlType("MonitorDiskSpace")]
//    public class MonitorDiskSpace : MonitorItem
//    {
//        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.DiskSpace;

//        [XmlAttribute("Drive")]
//        public string Drive { get; set; }

//        public override List<EventConditionSource> GetEventConditionSources()
//        {
//            return new List<EventConditionSource>()
//            {
//                EventConditionSource.Exception,
//                EventConditionSource.NoException,
//                EventConditionSource.DriveAvailableFreeSpace
//            };
//        }
//    }
//}
