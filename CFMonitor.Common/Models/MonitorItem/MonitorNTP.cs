//using CFMonitor.Enums;
//using System;
//using System.Collections.Generic;
//using System.Xml.Serialization;

//namespace CFMonitor.Models.MonitorItems
//{
//    /// <summary>
//    /// Settings for monitoring NTP time
//    /// </summary>
//    [XmlType("MonitorNTP")]
//    public class MonitorNTP : MonitorItem
//    {
//        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.NTP;

//        [XmlAttribute("Server")]
//        public string Server { get; set; }

//        [XmlAttribute("MaxToleranceSecs")]
//        public int MaxToleranceSecs { get; set; }

//        public override List<EventConditionSource> GetEventConditionSources()
//        {
//            return new List<EventConditionSource>()
//            {
//                EventConditionSource.Exception,
//                EventConditionSource.NoException,
//                EventConditionSource.NTPTimeInTolerance,
//                EventConditionSource.NTPTimeOutsideTolerance
//            };
//        }
//    }
//}
