using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CFMonitor
{
    /// <summary>
    /// Actions starting a process
    /// </summary>
    public class ActionerProcess : IActioner
    {
        public void DoAction(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            ActionProcess actionProcess = (ActionProcess)actionItem;
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = actionProcess.FileName;                     
            Process.Start(startInfo);
        }

        public bool CanAction(ActionItem actionItem)
        {
            return actionItem is ActionProcess;
        }
    }
}
