using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions sending Datadog warning
    /// </summary>
    public class ActionerDatadogWarning : ActionerBase, IActioner
    {        
        public ActionerDatadogWarning(IAuditEventFactory auditEventFactory, 
                            IAuditEventService auditEventService,
                            IAuditEventTypeService auditEventTypeService, 
                            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, systemValueTypeService)
        {
     
        }

        public string Name => "Create Datadog warning";

        //public ActionerTypes ActionerType => ActionerTypes.DatadogWarning;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> parameters)
        {
            //ActionDatadogWarning actionDatadogWarning = (ActionDatadogWarning)actionItem;

            throw new NotImplementedException();

            return Task.CompletedTask;
        }
         
        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem.ActionerType == ActionerTypes.DatadogWarning;
        }
    }
}
