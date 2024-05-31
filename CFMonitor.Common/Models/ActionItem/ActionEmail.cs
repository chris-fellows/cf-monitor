using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CFMonitor.Models.ActionItems
{
    /// <summary>
    /// Details for action to send email
    /// </summary>
    [XmlType("ActionEmail")]
    public class ActionEmail : ActionItem
    {
        [XmlAttribute("Subject")]
        public string Subject { get; set; }        
        [XmlAttribute("Sender")]
        public string Sender { get; set; }
        [XmlElement("Body")]
        public string Body { get; set; }
        [XmlArray("RecipientList")]
        [XmlArrayItem("Recipient")]
        public List<string> RecipientList = new List<string>();
        [XmlArray("CCList")]
        [XmlArrayItem("CC")]
        public List<string> CCList = new List<string>();
        [XmlAttribute("Server")]
        public string Server { get; set; }
        [XmlAttribute("UserName")]
        public string UserName { get; set; }
        [XmlAttribute("Password")]
        public string Password { get; set; }
    }
}
