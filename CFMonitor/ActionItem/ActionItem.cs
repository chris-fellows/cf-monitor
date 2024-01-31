using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CFMonitor
{
    [XmlType("ActionItem")]
    [XmlInclude(typeof(ActionEmail))]
    [XmlInclude(typeof(ActionProcess))]
    [XmlInclude(typeof(ActionURL))]
    [XmlInclude(typeof(ActionSQL))]
    [XmlInclude(typeof(ActionLog))]
    public abstract class ActionItem
    {
        
    }
}
