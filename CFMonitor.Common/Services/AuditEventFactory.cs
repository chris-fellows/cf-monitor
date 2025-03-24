using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Services
{
    public class AuditEventFactory : IAuditEventFactory
    {
        protected readonly IAuditEventService _auditEventService;
        protected readonly IAuditEventTypeService _auditEventTypeService;
        protected readonly ISystemValueTypeService _systemValueTypeService;

        public AuditEventFactory(IAuditEventService auditEventService,
                        IAuditEventTypeService auditEventTypeService,
                        ISystemValueTypeService systemValueTypeService)
        {
            _auditEventService = auditEventService;
            _auditEventTypeService = auditEventTypeService;
            _systemValueTypeService = systemValueTypeService;
        }

        public AuditEvent CreateActionExecuted(string monitorItemOutputId, string actionItemId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.ActionExecuted);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_MonitorItemOutputId).Id,
                        Value = monitorItemOutputId
                    },                   
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_ActionItemId).Id,
                        Value = actionItemId
                    },
                }
            };

            return auditEvent;
        }

        public AuditEvent CreateError(string errorMessage, List<AuditEventParameter> parameters)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.Error);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_ErrorMessage).Id,
                        Value = errorMessage
                    },                   
                }
            };
            auditEvent.Parameters.AddRange(parameters);

            return auditEvent;
        }

        public AuditEvent CreateCheckedMonitorItem(string monitorItemOutputId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.CheckedMonitorItem);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_MonitorItemOutputId).Id,
                        Value = monitorItemOutputId
                    }                    
                }
            };

            return auditEvent;
        }

        public AuditEvent CreateCheckingMonitorItem(string monitorAgentId, string monitorItemId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.CheckingMonitorItem);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_MonitorAgentId).Id,
                        Value = monitorAgentId
                    },
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_MonitorItemId).Id,
                        Value = monitorItemId
                    }
                }
            };

            return auditEvent;
        }

        public AuditEvent CreateUserAdded(string userId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.UserAdded);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_UserId).Id,
                        Value = userId
                    }                    
                }
            };

            return auditEvent;
        }

        public AuditEvent CreateMonitorItemAdded(string monitorItemId, string userId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.MonitorItemAdded);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_MonitorItemId).Id,
                        Value = monitorItemId
                    },
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_UserId).Id,
                        Value = userId
                    }
                }
            };
            if (String.IsNullOrEmpty(userId))
            {
                auditEvent.Parameters.RemoveAll(p => String.IsNullOrEmpty(p.Value));
            }

            return auditEvent;
        }

        public AuditEvent CreateMonitorItemUpdated(string monitorItemId, string userId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.MonitorItemUpdated);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_MonitorItemId).Id,
                        Value = monitorItemId
                    },
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_UserId).Id,
                        Value = userId
                    }
                }
            };
            if (String.IsNullOrEmpty(userId))
            {
                auditEvent.Parameters.RemoveAll(p => String.IsNullOrEmpty(p.Value));
            }

            return auditEvent;
        }

        public AuditEvent CreateMonitorAgentHeartbeat(string monitorAgentId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.MonitorAgentHeartbeat);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_MonitorAgentId).Id,
                        Value = monitorAgentId
                    }                  
                }
            };         

            return auditEvent;
        }

        public AuditEvent CreateMonitorAgentAdded(string monitorAgentId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.MonitorAgentAdded);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_MonitorAgentId).Id,
                        Value = monitorAgentId
                    }
                }
            };

            return auditEvent;
        }
    }
}
