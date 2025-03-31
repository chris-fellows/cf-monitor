using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;

namespace CFMonitor.Services
{
    public class EntityDependencyCheckerService : IEntityDependencyCheckerService
    {
        private readonly IMonitorAgentService _monitorAgentService;
        private readonly IMonitorItemService _monitorItemService;
        private readonly ISystemValueTypeService _systemValueTypeService;

        public EntityDependencyCheckerService(IMonitorAgentService monitorAgentService,
                    IMonitorItemService monitorItemService,
                    ISystemValueTypeService systemValueTypeService)
        {
            _monitorAgentService = monitorAgentService;
            _monitorItemService = monitorItemService;
            _systemValueTypeService = systemValueTypeService;
        }

        public async Task<bool> CanDelete(FileObject fileObject)
        {
            var systemValueTypes = await _systemValueTypeService.GetAllAsync();

            var monitorItems = await _monitorItemService.GetAllAsync();

            // Get System Value Type Ids that refer to FileObject.Id
            var fileObjectIdsSystemValueTypeIds = systemValueTypes.Where(svt => svt.EntityIdType != null &&
                                                                    svt.EntityIdType == EntityIdTypes.FileObjectId)
                                                .Select(svt => svt.Id).ToList();           

            var isDependentMonitorItems = monitorItems.Any(mi => mi.Parameters.Any(p => fileObjectIdsSystemValueTypeIds.Contains(p.SystemValueTypeId)));

            return !isDependentMonitorItems;
        }        

        public async Task<bool> CanDelete(MonitorAgentGroup monitorAgentGroup)
        {
            var isDependentMonitorAgents = (await _monitorAgentService.GetAllAsync()).Where(ma => ma.MonitorAgentGroupId == monitorAgentGroup.Id).Any();

            return !isDependentMonitorAgents;
        }

        public Task<bool> CanDelete(MonitorItem monitorItem)
        {
            // Currently always allow delete
            return Task.FromResult(true);
        }
    }
}
