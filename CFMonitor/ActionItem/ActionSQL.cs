using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
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
