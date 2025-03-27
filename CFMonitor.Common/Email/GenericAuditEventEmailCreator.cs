using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Email
{
    /// <summary>
    /// Creates generic email for audit event
    /// </summary>
    public class GenericAuditEventEmailCreator : IEmailCreator
    {
        public static string CreatorName => "GenericAuditEvent";
        
        public string Name => CreatorName;

        public List<string> GetRecipientEmails(List<SystemValue> systemValues)
        {
            return new List<string>();
        }

        public string GetSubject(List<SystemValue> systemValues)
        {
            return "";
        }
        
        public string GetBody(List<SystemValue> systemValues)
        {
            return "";
        }
    }
}
