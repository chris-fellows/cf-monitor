using CFMonitor.Models.MonitorItems;
using System.Collections.Generic;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Interface for component that checks a monitor item and, if the conditions are met, performs one or more
    /// actions
    /// </summary>
    public interface IChecker
    {
        void Check(MonitorItem monitorItem, List<IActioner> actionerList);
        bool CanCheck(MonitorItem monitorItem);
    }
}
