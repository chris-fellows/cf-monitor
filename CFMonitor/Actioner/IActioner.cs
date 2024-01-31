using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor
{
    /// <summary>
    /// Interface for component then performs an action when a monitor item has been checked and meets the
    /// conditions for the action
    /// </summary>
    public interface IActioner
    {
        void DoAction(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters);
        bool CanAction(ActionItem actionItem);
    }
}
