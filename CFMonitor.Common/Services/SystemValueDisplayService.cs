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
    public class SystemValueDisplayService : ISystemValueDisplayService
    {
        private readonly IAuditEventService _auditEventService;
        private readonly IAuditEventTypeService _auditEventTypeService;
        private readonly IContentTemplateService _contentTemplateService;
        private readonly IFileObjectService _fileObjectService;
        private readonly IMonitorAgentService _monitorAgentService;
        private readonly IMonitorAgentGroupService _monitorAgentGroupService;
        private readonly IMonitorAgentManagerService _monitorAgentManagerService;
        private readonly IMonitorItemOutputService _monitorItemOutputService;
        private readonly IMonitorItemService _monitorItemService;
        private readonly IMonitorItemTypeService _monitorItemTypeService;
        private readonly IPasswordResetService _passwordResetService;
        private readonly ISystemValueTypeService _systemValueTypeService;        
        private readonly IUserService _userService;

        public SystemValueDisplayService(IAuditEventService auditEventService,
                        IAuditEventTypeService auditEventTypeService,                        
                        IContentTemplateService contentTemplateService,
                        IFileObjectService fileObjectService,
                        IMonitorAgentService monitorAgentService,
                        IMonitorAgentGroupService monitorAgentGroupService,
                        IMonitorAgentManagerService monitorAgentManagerService,
                        IMonitorItemOutputService monitorItemOutputService,
                        IMonitorItemService monitorItemService,
                        IMonitorItemTypeService monitorItemTypeService,
                        IPasswordResetService passwordResetService,
                        ISystemValueTypeService systemValueTypeService,                        
                        IUserService userService)
        {
            _auditEventService = auditEventService;
            _auditEventTypeService = auditEventTypeService;
            _contentTemplateService = contentTemplateService;
            _fileObjectService = fileObjectService;
            _monitorAgentService = monitorAgentService;
            _monitorAgentGroupService = monitorAgentGroupService;
            _monitorAgentManagerService = monitorAgentManagerService;
            _monitorItemOutputService = monitorItemOutputService;
            _monitorItemService = monitorItemService;
            _monitorItemTypeService = monitorItemTypeService;
            _passwordResetService = passwordResetService;
            _systemValueTypeService = systemValueTypeService;            
            _userService = userService;
        }

        public async Task<List<string[]>> GetDisplayItemsAsync(SystemValue systemValue)
        {
            // Get system value type
            var systemValueType = await _systemValueTypeService.GetByIdAsync(systemValue.TypeId);            
            
            // Check EntityIdType, may refer to one of our entities
            if (systemValueType.EntityIdType != null)
            {
                switch(systemValueType.EntityIdType)
                {
                    case EntityIdTypes.AuditEventTypeId:
                        var auditEventType = await _auditEventTypeService.GetByIdAsync(systemValue.Value);
                        return new List<string[]>
                        {
                            new[] { "Audit Event Type", auditEventType.Name }
                        };

                    case EntityIdTypes.ContentTemplateId:
                        var contentTemplate = await _contentTemplateService.GetByIdAsync(systemValue.Value);
                        return new List<string[]>
                        {
                            new[] { "Content Template", contentTemplate.Name }
                        };

                    case EntityIdTypes.FileObjectId:
                        var fileObject = await _fileObjectService.GetByIdAsync(systemValue.Value);
                        return new List<string[]>
                        {
                            new[] { "File", fileObject.Name }
                        };

                    case EntityIdTypes.MonitorAgentId:
                        var monitorAgent = await _monitorAgentService.GetByIdAsync(systemValue.Value);
                        return new List<string[]>()
                        {
                            new[] { "Monitor Agent Machine", monitorAgent.MachineName }
                        };

                    case EntityIdTypes.MonitorAgentGroupId:
                        var monitorAgentGroup = await _monitorAgentGroupService.GetByIdAsync(systemValue.Value);
                        return new List<string[]>()
                        {
                            new[] { "Monitor Agent Group", monitorAgentGroup.Name }
                        };

                    case EntityIdTypes.MonitorAgentManagerId:
                        var monitorAgentManager = await _monitorAgentManagerService.GetByIdAsync(systemValue.Value);
                        return new List<string[]>()
                        {
                            new[] { "Monitor Agent Manager Machine", monitorAgentManager.MachineName }
                        };

                    case EntityIdTypes.MonitorItemId:
                        var monitorItem = await _monitorItemService.GetByIdAsync(systemValue.Value);
                        return new List<string[]>
                        {
                            new [] { "Monitor Item", monitorItem.Name }
                        };

                    case EntityIdTypes.MonitorItemTypeId:
                        var monitorItemType = _monitorItemTypeService.GetAll().First(t => t.Id == systemValue.Value);
                        return new List<string[]>
                        {
                            new [] { "Monitor Item Type", monitorItemType.Name }
                        };

                    case EntityIdTypes.MonitorItemOutputId:
                        {
                            var monitorItemOutput = await _monitorItemOutputService.GetByIdAsync(systemValue.Value);
                            var monitorItem2 = await _monitorItemService.GetByIdAsync(monitorItemOutput.MonitorItemId);
                            var monitorAgent2 = await _monitorAgentService.GetByIdAsync(monitorItemOutput.MonitorAgentId);
                            return new List<string[]>
                            {
                                new [] { "Monitor Item", monitorItem2.Name },
                                new [] { "Monitor Agent Machine", monitorAgent2.MachineName },
                                new [] { "Monitor Checked", monitorItemOutput.CheckedDateTime.ToString() },  // TODO: Format this
                                new [] { "Actions", monitorItemOutput.EventItemIdsForAction.Count.ToString() }
                            };
                        }                        

                    case EntityIdTypes.UserId:
                        var user = await _userService.GetByIdAsync(systemValue.Value);
                        return new List<string[]>()
                        {
                            new[] { "User", user.Name }
                        };
                }
            }

            // Return default
            return new List<string[]> {
                            new[] { systemValueType.Name, systemValue.Value }
                        };           
        }

    }
}
