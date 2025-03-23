using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFUtilities.Interfaces;
using CFUtilities.Models;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions sending an email
    /// </summary>
    public class ActionerEmail : ActionerBase, IActioner
    {
        private readonly IPlaceholderService _placeholderService;        

        public ActionerEmail(IAuditEventFactory auditEventFactory,
                            IAuditEventService auditEventService,
                            IAuditEventTypeService auditEventTypeService, 
                            IPlaceholderService placeholderService,
                            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, systemValueTypeService)
        {
            _placeholderService = placeholderService;            
        }

        public string Name => "Send an email";

        //public ActionerTypes ActionerType => ActionerTypes.Email;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> parameters)
        {
            var systemValueTypes = _systemValueTypeService.GetAll();
            
            var bodyParam = actionItem.Parameters.First(p => p.SystemValueTypeId == systemValueTypes.First(svt => svt.ValueType ==  SystemValueTypes.AIP_EmailBody).Id);
            var serverParam = actionItem.Parameters.First(p => p.SystemValueTypeId == systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIP_EmailServer).Id);
            var subjectParam = actionItem.Parameters.First(p => p.SystemValueTypeId == systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIP_EmailSubject).Id);
            var recipientParams = actionItem.Parameters.Where(p => p.SystemValueTypeId == systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIP_EmailRecipient).Id);
            var ccParams = actionItem.Parameters.Where(p => p.SystemValueTypeId == systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIP_EmailCC).Id);

            var errorMessageParam = parameters.FirstOrDefault(p => p.SystemValueTypeId == systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIPC_ErrorMessage).Id);

            // Add custom placeholders to replace error message
            var placeholderValues = new Dictionary<string, object>();
            _placeholderService.ResetDefaults();    // Reset default placeholders
            if (errorMessageParam != null)
            {
                var errorMessagePlaceholder = new Placeholder()
                {
                    Name = "{error_message}",
                    CanGetValue = (placeholderName) => placeholderName.Equals("{error_message}"),
                    GetValue = (placeholderName, placeholderParameters) =>
                    {
                        return placeholderParameters[placeholderName].ToString()!;
                    }
                };
                _placeholderService.Add(errorMessagePlaceholder);

                // Add placeholder value to replace
                placeholderValues.Add(errorMessagePlaceholder.Name, errorMessageParam.Value);
            }
            
            MailMessage message = new MailMessage()
            {
                Subject = _placeholderService.GetWithPlaceholdersReplaced(subjectParam.Value, placeholderValues),
                Body = _placeholderService.GetWithPlaceholdersReplaced(bodyParam.Value, placeholderValues),
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
