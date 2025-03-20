using CFMonitor.Enums;
using CFMonitor.Models.MonitorItems;

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
        /// Internal name
        /// </summary>
        CheckerTypes CheckerType { get; }

        /// <summary>
        /// Checks monitor item
        /// </summary>
        /// <param name="monitorItem"></param>
        /// <param name="actionerList"></param>
        /// <param name="testMode">Whether test mode enabled (Testing without executing actions)</param>
        Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode);

        /// <summary>
        /// Determines if this instance can check
        /// </summary>
        /// <param name="monitorItem"></param>
        /// <returns></returns>
        bool CanCheck(MonitorItem monitorItem);
    }
}
