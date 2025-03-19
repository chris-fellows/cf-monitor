//using CFMonitor.Enums;
//using System;
//using System.Collections.Generic;
//using System.Xml.Serialization;

//namespace CFMonitor.Models.MonitorItems
//{
//    [XmlType("MonitorMemory")]
//    public class MonitorMemory : MonitorItem
//    {
//        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.Memory;

//        public override List<EventConditionSource> GetEventConditionSources()
//        {
//            return new List<EventConditionSource>()
//            {
//                EventConditionSource.Exception,
//                EventConditionSource.NoException                
//            };
//        }
//    }
//}
