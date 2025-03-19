//using CFMonitor.Enums;
//using System;
//using System.Collections.Generic;
//using System.Xml.Serialization;

//namespace CFMonitor.Models.MonitorItems
//{
//    [XmlType("MonitorIMAP")]
//    public class MonitorIMAP : MonitorItem
//    {
//        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.IMAP;

//        public override List<EventConditionSource> GetEventConditionSources()
//        {
//            return new List<EventConditionSource>()
//            {
//                EventConditionSource.Exception,
//                EventConditionSource.NoException,
//                EventConditionSource.IMAPConnected,
//                EventConditionSource.IMAPConnectError
//            };
//        }
//    }
//}
