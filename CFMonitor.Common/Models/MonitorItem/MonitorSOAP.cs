//using CFMonitor.Enums;
//using System;
//using System.Collections.Generic;
//using System.Xml.Serialization;

//namespace CFMonitor.Models.MonitorItems
//{
//    [XmlType("MonitorSOAP")]
//    public class MonitorSOAP : MonitorItem
//    {
//        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.SOAP;

//        [XmlAttribute("URL")]
//        public string URL { get; set; }
//        [XmlElement("XML")]
//        public string XML { get; set; }

//        public override List<EventConditionSource> GetEventConditionSources()
//        {
//            return new List<EventConditionSource>()
//            {
//                EventConditionSource.Exception,
//                EventConditionSource.NoException,
//                EventConditionSource.HttpResponseStatusCode,
//                EventConditionSource.WebExceptionStatus
//            };
//        }
//    }
//}
