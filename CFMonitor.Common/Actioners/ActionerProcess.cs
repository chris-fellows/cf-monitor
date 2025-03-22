using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions starting a process
    /// </summary>
    public class ActionerProcess : IActioner
    {
        private readonly ISystemValueTypeService _systemValueTypeService;

        public ActionerProcess(ISystemValueTypeService systemValueTypeService)
        {
            _systemValueTypeService = systemValueTypeService;
        }

        public string Name => "Run process";

        //public ActionerTypes ActionerType => ActionerTypes.Process;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> parameters)
        {
            var systemValueTypes = _systemValueTypeService.GetAll();

            //ActionProcess actionProcess = (ActionProcess)actionItem;
            var filenameParam = actionItem.Parameters.First(p => p.SystemValueTypeId == systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIP_ProcessFileName).Id);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = filenameParam.Value;
            var process = Process.Start(startInfo);

            // Wait for completion
            process.WaitForExit();

            
            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem.ActionerType == ActionerTypes.Process;
        }
    }
}
