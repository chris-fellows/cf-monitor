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
    /// Creates email for reset password
    /// </summary>
    public class ResetPasswordEmailCreator : IEmailCreator
    {
        public static string CreatorName => "ResetPassword";
     
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
