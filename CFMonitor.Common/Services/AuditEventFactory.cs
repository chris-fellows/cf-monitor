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

        public AuditEvent CreateActionExecuted(string createdUserId, string monitorItemOutputId, string actionItemId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.ActionExecuted);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
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

        public AuditEvent CreateError(string createdUserId, string errorMessage, List<AuditEventParameter> parameters)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.Error);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
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

        public AuditEvent CreateCheckedMonitorItem(string createdUserId, string monitorItemOutputId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.CheckedMonitorItem);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
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

        public AuditEvent CreateCheckingMonitorItem(string createdUserId, string monitorAgentId, string monitorItemId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.CheckingMonitorItem);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
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

        public AuditEvent CreateUserAdded(string createdUserId, string userId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.UserAdded);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
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

        public AuditEvent CreateMonitorItemAdded(string createdUserId, string monitorItemId, string userId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.MonitorItemAdded);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
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

        public AuditEvent CreateMonitorItemUpdated(string createdUserId, string monitorItemId, string userId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.MonitorItemUpdated);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
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

        public AuditEvent CreateMonitorAgentHeartbeat(string createdUserId, string monitorAgentId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.MonitorAgentHeartbeat);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
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

        public AuditEvent CreateMonitorAgentAdded(string createdUserId, string monitorAgentId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.MonitorAgentAdded);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
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

        public AuditEvent CreatePasswordResetAdded(string createdUserId, string passwordResetId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.PasswordResetCreated);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_PasswordResetId).Id,
                        Value = passwordResetId
                    }
                }
            };

            return auditEvent;
        }

        public AuditEvent CreateUserLogInSuccess(string createdUserId, string userId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.UserLogInSuccess);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
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

        public AuditEvent CreateUserLogOut(string createdUserId, string userId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.UserLogOut);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
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

        public AuditEvent CreateUserLogInError(string createdUserId, string username)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.UserLogInError);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
                Parameters = new List<AuditEventParameter>()
                {
                    new AuditEventParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_Notes).Id,
                        Value = username
                    }
                }
            };

            return auditEvent;
        }

        public AuditEvent CreatePasswordUpdated(string createdUserId, string userId)
        {
            var auditEventType = _auditEventTypeService.GetAll().First(aet => aet.EventType == AuditEventTypes.PasswordUpdated);
            var systemValueTypes = _systemValueTypeService.GetAll();

            var auditEvent = new AuditEvent()
            {
                Id = Guid.NewGuid().ToString(),
                TypeId = auditEventType.Id,
                CreatedDateTime = DateTimeOffset.UtcNow,
                CreatedUserId = createdUserId,
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
    }
}
