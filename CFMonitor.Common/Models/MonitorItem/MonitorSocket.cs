using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    [XmlType("MonitorSocket")]
    public class MonitorSocket : MonitorItem
    {
        [XmlAttribute("Port")]
        public int Port { get; set; }
        [XmlAttribute("Host")]
        public string Host { get; set; }
        [XmlAttribute("Protocol")]
        public string Protocol { get; set; }            // TCP/UDP

        public override List<EventConditionSource> GetEventConditionSources()
        {
            return new List<EventConditionSource>()
            {
                EventConditionSource.Exception,
                EventConditionSource.NoException,
                EventConditionSource.SocketConnected,
                EventConditionSource.SocketNotConnected
            };
        }
    }
}
