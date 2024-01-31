using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CFMonitor
{
    /// <summary>
    /// Actions writing to log file
    /// </summary>
    public class ActionerLog : IActioner
    {
        public void DoAction(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {


        }

        public bool CanAction(ActionItem actionItem)
        {
            return actionItem is ActionLog;
        }
    }
}
