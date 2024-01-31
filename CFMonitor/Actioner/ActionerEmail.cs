using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace CFMonitor
{
    /// <summary>
    /// Actions sending an email
    /// </summary>
    public class ActionerEmail : IActioner
    {
        public void DoAction(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            ActionEmail actionEmail = (ActionEmail)actionItem;
            MailMessage message = new MailMessage()
            {
                Subject = actionEmail.Subject,
                Body = actionParameters.Values["Body"].ToString(),
                IsBodyHtml = true,               
            };

            foreach(string rcecipient in actionEmail.RecipientList)
            {
                message.To.Add(new MailAddress(rcecipient));
            }
       
            foreach(string cc in actionEmail.CCList)
            {
                message.CC.Add(new MailAddress(cc));
            }

            SmtpClient client = new SmtpClient(actionEmail.Server);
            client.Send(message);
        }

        public bool CanAction(ActionItem actionItem)
        {
            return actionItem is ActionEmail;
        }
    }
}
