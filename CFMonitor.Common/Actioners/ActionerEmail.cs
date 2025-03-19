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
            //ActionEmail actionEmail = (ActionEmail)actionItem;
            var bodyParam = actionItem.Parameters.First(p => p.SystemValueType == SystemValueTypes.AIP_EmailBody);
            var serverParam = actionItem.Parameters.First(p => p.SystemValueType == SystemValueTypes.AIP_EmailServer);
            var subjectParam = actionItem.Parameters.First(p => p.SystemValueType == SystemValueTypes.AIP_EmailSubject);
            var recipientParams = actionItem.Parameters.Where(p => p.SystemValueType == SystemValueTypes.AIP_EmailRecipient);
            var ccParams = actionItem.Parameters.Where(p => p.SystemValueType == SystemValueTypes.AIP_EmailCC);

            MailMessage message = new MailMessage()
            {
                Subject = subjectParam.Value,
                Body = actionParameters.Values[ActionParameterTypes.Body].ToString(),
                IsBodyHtml = true,               
            };

            foreach(var recipientParam in recipientParams)
            {
                message.To.Add(new MailAddress(recipientParam.Value));
            }
       
            foreach(var ccParam in ccParams)
            {
                message.CC.Add(new MailAddress(ccParam.Value));
            }

            SmtpClient client = new SmtpClient(serverParam.Value);
            client.Send(message);

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem.ActionerType == ActionerTypes.Email;
        }
    }
}
