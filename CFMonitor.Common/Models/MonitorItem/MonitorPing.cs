//using CFMonitor.Enums;
//using System;
//using System.Collections.Generic;
//using System.Xml.Serialization;

//namespace CFMonitor.Models.MonitorItems
//{
//    /// <summary>
//    /// Settings for monitoring a server by ping
//    /// </summary>
//    [XmlType("MonitorPing")]
//    public class MonitorPing : MonitorItem
//    {
//        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.Ping;

//        [XmlAttribute("Server")]
//        public string Server { get; set; }

//        public override List<EventConditionSource> GetEventConditionSources()
//        {
//            return new List<EventConditionSource>()
//            {
//                EventConditionSource.Exception,
//                EventConditionSource.NoException,
//                EventConditionSource.PingReplyStatus                
//            };
//        }
//    }
//}
