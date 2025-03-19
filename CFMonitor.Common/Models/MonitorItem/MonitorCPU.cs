//using CFMonitor.Enums;
//using System;
//using System.Collections.Generic;
//using System.Xml.Serialization;

//namespace CFMonitor.Models.MonitorItems
//{
//    /// <summary>
//    /// Settings for monitoring CPU
//    /// </summary>
//    [XmlType("MonitorCPU")]
//    public class MonitorCPU : MonitorItem
//    {
//        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.CPU;

//        [XmlAttribute("Server")]
//        public string Server { get; set; }

//        public override List<EventConditionSource> GetEventConditionSources()
//        {
//            return new List<EventConditionSource>()
//            {
//                EventConditionSource.Exception,
//                EventConditionSource.NoException,
//                EventConditionSource.CPUInTolerance,
//                EventConditionSource.CPUOutsideTolerance
//            };
//        }
//    }
//}
