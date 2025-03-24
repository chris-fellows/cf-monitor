using CFMonitor.Models;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Interface for component that checks a monitor item and, if the conditions are met, performs one or more
    /// actions
    /// </summary>
    public interface IChecker
    {
        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Checks monitor item
        /// </summary>
        /// <param name="monitorItem"></param>
        /// <param name="actionerList"></param>
        /// <param name="testMode">Whether test mode enabled (Testing without executing actions)</param>
        Task<MonitorItemOutput> CheckAsync(MonitorAgent monitorAgent, MonitorItem monitorItem, CheckerConfig checkerConfig);

        /// <summary>
        /// Determines if this instance can check monitor item
        /// </summary>
        /// <param name="monitorItem"></param>
        /// <returns></returns>
        bool CanCheck(MonitorItem monitorItem);
    }
}
