using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    public abstract class ActionerBase
    {
        protected readonly IAuditEventFactory _auditEventFactory;
        protected readonly IAuditEventService _auditEventService;
        protected readonly IAuditEventTypeService _auditEventTypeService;
        protected readonly ISystemValueTypeService _systemValueTypeService;

        public ActionerBase(IAuditEventFactory auditEventFactory,
                        IAuditEventService auditEventService,
                        IAuditEventTypeService auditEventTypeService, 
                        ISystemValueTypeService systemValueTypeService)
        {
            _auditEventFactory = auditEventFactory;
            _auditEventService = auditEventService;
            _auditEventTypeService = auditEventTypeService;
            _systemValueTypeService = systemValueTypeService;
        }

        protected void AddAuditEventActionExecuted(MonitorAgent monitorAgent, MonitorItem monitorItem, ActionItem actionItem)
        {
            var auditEvent = _auditEventFactory.CreateActionExecuted(monitorAgent.Id, monitorItem.Id, actionItem.Id);
            _auditEventService.Add(auditEvent);
        }
    }
}
