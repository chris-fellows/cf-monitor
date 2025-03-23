using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions writing to log file
    /// </summary>
    public class ActionerLog : ActionerBase, IActioner
    {        
        public ActionerLog(IAuditEventFactory auditEventFactory, 
                            IAuditEventService auditEventService,
                            IAuditEventTypeService auditEventTypeService, 
                            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, systemValueTypeService)
        {
     
        }


        public string Name => "Write a log";

        //public ActionerTypes ActionerType => ActionerTypes.Log;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> parameters)
        {
            throw new NotImplementedException();

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem.ActionerType == ActionerTypes.Log;
        }
    }
}
