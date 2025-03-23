using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions sending SMS text
    /// </summary>
    public class ActionerSMS : ActionerBase, IActioner
    {        
        public ActionerSMS(IAuditEventFactory auditEventFactory, 
                            IAuditEventService auditEventService,
                            IAuditEventTypeService auditEventTypeService, 
                            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, systemValueTypeService)
        {
     
        }

        public string Name => "Send SMS text";

        //public ActionerTypes ActionerType => ActionerTypes.SMS;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> parameters)
        {
            //ActionSMS actionSMS = (ActionSMS)actionItem;

            throw new NotImplementedException();

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem.ActionerType == ActionerTypes.SMS;
        }
    }
}
