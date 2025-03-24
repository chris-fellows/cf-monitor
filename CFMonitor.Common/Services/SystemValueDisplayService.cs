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
        private readonly IMonitorAgentService _monitorAgentService;
        private readonly IMonitorItemOutputService _monitorItemOutputService;
        private readonly IMonitorItemService _monitorItemService;
        private readonly ISystemValueTypeService _systemValueTypeService;        
        private readonly IUserService _userService;

        public SystemValueDisplayService(IAuditEventService auditEventService,
                        IAuditEventTypeService auditEventTypeService,                        
                        IMonitorAgentService monitorAgentService,
                        IMonitorItemOutputService monitorItemOutputService,
                        IMonitorItemService monitorItemService,
                        ISystemValueTypeService systemValueTypeService,                        
                        IUserService userService)
        {
            _auditEventService = auditEventService;
            _auditEventTypeService = auditEventTypeService;
            _monitorAgentService = monitorAgentService;
            _monitorItemOutputService = monitorItemOutputService;
            _monitorItemService = monitorItemService;
            _systemValueTypeService = systemValueTypeService;            
            _userService = userService;
        }

        public async Task<List<string[]>> GetDisplayItemsAsync(SystemValue systemValue)
        {
            // Get system value type
            var systemValueType = _systemValueTypeService.GetById(systemValue.TypeId);            

            // Set function to get display value from value
            // TODO: Can we store the label in SystemValueType?
            var displayFunction = new Dictionary<SystemValueTypes, Func<string, Task<List<string[]>>>>
            {                            
                { SystemValueTypes.AEP_MonitorAgentId, async (value) =>
                    {
                        var monitorAgent = _monitorAgentService.GetById(value);
                        return new List<string[]>()
                        {
                            new[] { "Monitor Agent Machine", monitorAgent.MachineName }
                        };
                    }
                },
                { SystemValueTypes.AEP_MonitorItemId, async (value) =>
                    {
                        var monitorItem = _monitorItemService.GetById(value);
                        return new List<string[]>
                        {
                            new [] { "Monitor Item", monitorItem.Name }
                        };
                    }
                },
                { SystemValueTypes.AEP_MonitorItemOutputId, async (value) =>
                    {
                        var monitorItemOutput = _monitorItemOutputService.GetById(value);
                        var monitorItem = _monitorItemService.GetById(monitorItemOutput.MonitorItemId);
                        var monitorAgent = _monitorAgentService.GetById(monitorItemOutput.MonitorAgentId);
                        return new List<string[]>
                        {
                            new [] { "Monitor Item", monitorItem.Name },
                            new [] { "Monitor Agent Machine", monitorAgent.MachineName },
                            new [] { "Monitor Checked", monitorItemOutput.CheckedDateTime.ToString() },  // TODO: Format this
                            new [] { "Actions", monitorItemOutput.EventItemIdsForAction.Count.ToString() }
                        };
                    }
                },

                { SystemValueTypes.AEP_UserId, async (value) =>
                    {
                        var user = _userService.GetById(value);
                        return new List<string[]>()
                        {
                            new[] { "User", user.Name }
                        };
                    }
                }
            };

            return displayFunction.ContainsKey(systemValueType.ValueType) ?
                        await displayFunction[systemValueType.ValueType](systemValue.Value) :
                        new List<string[]> {
                            new[] { systemValueType.Name, systemValue.Value }
                        };
        }

    }
}
