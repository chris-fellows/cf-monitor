using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions restart of machine
    /// </summary>
    public class ActionerMachineRestart : ActionerBase, IActioner
    {        
        public ActionerMachineRestart(IAuditEventFactory auditEventFactory, 
                            IAuditEventService auditEventService,
                            IAuditEventTypeService auditEventTypeService, 
                            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, systemValueTypeService)
        {
          
        }

        public string Name => "Restart machine";

        //public ActionerTypes ActionerType => ActionerTypes.MachineRestart;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> parameters)
        {
            //ActionMachineRestart actionMachineRestart = (ActionMachineRestart)actionItem;

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem.ActionerType == ActionerTypes.MachineRestart;
        }
    }
}
