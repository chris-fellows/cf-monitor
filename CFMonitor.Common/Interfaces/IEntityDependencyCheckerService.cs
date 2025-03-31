using CFMonitor.Models;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Entity depdendency checker service
    /// </summary>
    public interface IEntityDependencyCheckerService
    {
        Task<bool> CanDelete(FileObject fileObject);

        Task<bool> CanDelete(MonitorAgentGroup monitorAgentGroup);

        Task<bool> CanDelete(MonitorItem monitorItem);
    }
}
