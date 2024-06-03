using CFMonitor.Enums;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.MonitorItems
{
    /// <summary>
    /// Settings for monitoring a SQL database by running a query
    /// </summary>
    [XmlType("MonitorSQL")]
    public class MonitorSQL : MonitorItem
    {
        public override MonitorItemTypes MonitorItemType => MonitorItemTypes.SQL;

        [XmlAttribute("ConnectionString")]
        public string ConnectionString { get; set; }
        [XmlAttribute("QueryFile")]
        public string QueryFile { get; set; }

        public override List<EventConditionSource> GetEventConditionSources()
        {
            return new List<EventConditionSource>()
            {
                EventConditionSource.Exception,
                EventConditionSource.NoException,
                EventConditionSource.SQLReturnsRows,
                EventConditionSource.SQLReturnsNoRows
            };
        }
    }
}
