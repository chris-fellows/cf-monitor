using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions starting a process
    /// </summary>
    public class ActionerProcess : IActioner
    {
        public string Name => "Run process";

        public ActionerTypes ActionerType => ActionerTypes.Process;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            ActionProcess actionProcess = (ActionProcess)actionItem;
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = actionProcess.FileName;                     
            var process = Process.Start(startInfo);

            // Wait for completion
            process.WaitForExit();

            
            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem is ActionProcess;
        }
    }
}
