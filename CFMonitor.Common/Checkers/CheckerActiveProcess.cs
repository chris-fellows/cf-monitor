using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks active process
    /// </summary>
    public class CheckerActiveProcess : IChecker
    {
        private readonly ISystemValueTypeService _systemValueTypeService;

        public CheckerActiveProcess(ISystemValueTypeService systemValueTypeService)
        {
            _systemValueTypeService = systemValueTypeService;
        }

        public string Name => "Active Process";

        public CheckerTypes CheckerType => CheckerTypes.ActiveProcess;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            var systemValueTypes = _systemValueTypeService.GetAll();

            var svtFileName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_ActiveProcessFileName);             
            var fileNameParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtFileName.Id);

            var svtMachineName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_ActiveProcessMachineName);
            var machineNameParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtMachineName.Id);
            
            Exception exception = null;          
            ActionParameters actionParameters = new ActionParameters();
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
            }

            try
            {
                // Check events
                actionParameters.Values.Add(ActionParameterTypes.Body, "Error checking service");
                CheckEvents(actionerList, monitorItem, actionParameters, exception, processesFound);
            }
            catch (System.Exception ex)
            {

            }

            return Task.CompletedTask;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorItem monitorProcess, ActionParameters actionParameters, Exception exception, List<Process> processesFound)
        {
            foreach (EventItem eventItem in monitorProcess.EventItems)
            {
                bool meetsCondition = false;

                switch (eventItem.EventCondition.Source)
                {
                    case EventConditionSources.Exception:
                        meetsCondition = (exception != null);
                        break;
                    case EventConditionSources.NoException:
                        meetsCondition = (exception == null);
                        break;
                    case EventConditionSources.ActiveProcessRunning:
                        meetsCondition = (processesFound.Count > 0);
                        break;
                    case EventConditionSources.ActiveProcessNotRunning:
                        meetsCondition = (processesFound.Count == 0);
                        break;
                }          

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        DoAction(actionerList, monitorProcess, actionItem, actionParameters);
                    }
                }
            }
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.ActiveProcess;
            //return monitorItem is MonitorActiveProcess;
        }

        private void DoAction(List<IActioner> actionerList, MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            foreach (IActioner actioner in actionerList)
            {
                if (actioner.CanExecute(actionItem))
                {
                    actioner.ExecuteAsync(monitorItem, actionItem, actionParameters);
                    break;
                }
            }
        }
    }
}
