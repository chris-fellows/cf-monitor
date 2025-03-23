using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Base checker
    /// </summary>
    public abstract class CheckerBase
    {
        protected readonly IAuditEventFactory _auditEventFactory;
        protected readonly IAuditEventService _auditEventService;
        protected readonly IAuditEventTypeService _auditEventTypeService;
        protected readonly IEventItemService _eventItemService;
        protected readonly ISystemValueTypeService _systemValueTypeService;

        public CheckerBase(IAuditEventFactory auditEventFactory,
                        IAuditEventService auditEventService,
                        IAuditEventTypeService auditEventTypeService,
                        IEventItemService eventItemService, 
                        ISystemValueTypeService systemValueTypeService)
        {
            _auditEventFactory = auditEventFactory;
            _auditEventService = auditEventService;
            _auditEventTypeService = auditEventTypeService;
            _eventItemService = eventItemService;
            _systemValueTypeService = systemValueTypeService;
        }

        ///// <summary>
        ///// Executes action
        ///// </summary>
        ///// <param name="actionerList"></param>
        ///// <param name="monitorItem"></param>
        ///// <param name="actionItem"></param>
        ///// <param name="actionItemParameters"></param>
        //protected async Task ExecuteActionAsync(List<IActioner> actionerList, MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> actionItemParameters)
        //{
        //    foreach (IActioner actioner in actionerList)
        //    {
        //        if (actioner.CanExecute(actionItem))
        //        {
        //            await actioner.ExecuteAsync(monitorItem, actionItem, actionItemParameters);
        //            break;
        //        }
        //    }
        //}

        protected void AddAuditEventMonitorItemChecking(MonitorAgent monitorAgent, MonitorItem monitorItem)
        {
            var auditEvent = _auditEventFactory.CreateCheckingMonitorItem(monitorAgent.Id, monitorItem.Id);
            _auditEventService.Add(auditEvent);
        }

        protected void AddAuditEventMonitorItemChecked(MonitorAgent monitorAgent, MonitorItem monitorItem)
        {
            var auditEvent = _auditEventFactory.CreateCheckedMonitorItem(monitorAgent.Id, monitorItem.Id);
            _auditEventService.Add(auditEvent);            
        }
    }
}
