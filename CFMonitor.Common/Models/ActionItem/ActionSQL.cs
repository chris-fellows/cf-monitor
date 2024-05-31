using System.Xml.Serialization;

namespace CFMonitor.Models.ActionItems
{
    /// <summary>
    /// Details for action to run SQL
    /// </summary>
     [XmlType("ActionSQL")]
    public class ActionSQL : ActionItem
    {
        [XmlAttribute("ConnectionString")]
        public string ConnectionString;
        [XmlElement("SQL")]
        public string SQL;        
    }
}
