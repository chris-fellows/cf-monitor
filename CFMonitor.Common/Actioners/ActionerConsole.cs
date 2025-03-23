using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions writing to console
    /// </summary>
    public class ActionerConsole : ActionerBase, IActioner
    {
        public ActionerConsole(IAuditEventFactory auditEventFactory,
                            IAuditEventService auditEventService,
                            IAuditEventTypeService auditEventTypeService,
                            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, systemValueTypeService)
        {
     
        }

        public string Name => "Write to console";

        //public ActionerTypes ActionerType => ActionerTypes.Console;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> parameters)
        {
            throw new NotImplementedException();

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem.ActionerType == ActionerTypes.Console;
        }
    }
}
