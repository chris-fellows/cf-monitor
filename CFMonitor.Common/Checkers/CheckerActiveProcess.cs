using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks active process
    /// </summary>
    public class CheckerActiveProcess : CheckerBase, IChecker
    {        
        public CheckerActiveProcess(IEventItemService eventItemService,
            ISystemValueTypeService systemValueTypeService) : base(eventItemService, systemValueTypeService)
        {
     
        }

        public string Name => "Active Process";

        //public CheckerTypes CheckerType => CheckerTypes.ActiveProcess;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            return Task.Factory.StartNew(async () =>
            {
                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return;
                }

                var systemValueTypes = _systemValueTypeService.GetAll();

                var svtFileName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_ActiveProcessFileName);
                var fileNameParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtFileName.Id);

                var svtMachineName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_ActiveProcessMachineName);
                var machineNameParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtMachineName.Id);

                Exception exception = null;
                var actionItemParameters = new List<ActionItemParameter>();
                List<Process> processesFound = new List<Process>();

                try
                {
                    Process[] processes = String.IsNullOrEmpty(machineNameParam.Value) ?
                            Process.GetProcesses() : Process.GetProcesses(machineNameParam.Value);
                    foreach (Process process in processes)
                    {
                        string filePath = process.MainModule.FileName;
                        if (filePath.Equals(fileNameParam.Value, StringComparison.InvariantCultureIgnoreCase))
                        {
                            processesFound.Add(process);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    exception = ex;

                    actionItemParameters.Add(new ActionItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIPC_ErrorMessage).Id,
                        Value = ex.Message
                    });
                }

                try
                {
                    // Check events                
                    foreach (var eventItem in eventItems)
                    {
                        await CheckEventAsync(eventItem, actionerList, monitorItem, actionItemParameters, exception, processesFound);
                    }
                }
                catch (System.Exception ex)
                {

                }
            });
        }

        private async Task CheckEventAsync(EventItem eventItem, List<IActioner> actionerList, MonitorItem monitorProcess, List<ActionItemParameter> actionItemParameters, Exception exception, List<Process> processesFound)
        {            
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);                        
                        break;
                    case SystemValueTypes.ECS_ActiveProcessRunning:
                        meetsCondition = eventItem.EventCondition.IsValid(processesFound.Count > 0);
                        break;
                }          

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        await ExecuteActionAsync(actionerList, monitorProcess, actionItem, actionItemParameters);
                    }
                }            
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.ActiveProcess;
            //return monitorItem is MonitorActiveProcess;
        }    
    }
}
