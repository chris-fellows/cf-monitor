using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions sending an email
    /// </summary>
    public class ActionerEmail : IActioner
    {
        public string Name => "Send an email";

        public ActionerTypes ActionerType => ActionerTypes.Email;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            ActionEmail actionEmail = (ActionEmail)actionItem;
            MailMessage message = new MailMessage()
            {
                Subject = actionEmail.Subject,
                Body = actionParameters.Values[ActionParameterTypes.Body].ToString(),
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

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem is ActionEmail;
        }
    }
}
