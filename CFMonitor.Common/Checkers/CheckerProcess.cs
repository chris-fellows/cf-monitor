using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CFMonitor
{
    /// <summary>
    /// Checks process
    /// </summary>
    public class CheckerProcess : IChecker
    {
        public void Check(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorProcess monitorProcess = (MonitorProcess)monitorItem;
            Exception exception = null;          
            ActionParameters actionParameters = new ActionParameters();
            List<Process> processesFound = new List<Process>();

            try
            {
                Process[] processes = String.IsNullOrEmpty(monitorProcess.MachineName) ? Process.GetProcesses() : Process.GetProcesses(monitorProcess.MachineName);
                foreach (Process process in processes)
                {
                    string filePath = process.MainModule.FileName;
                    if (filePath.Equals(monitorProcess.FileName, StringComparison.InvariantCultureIgnoreCase))
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
                actionParameters.Values.Add("Body", "Error checking service");
                CheckEvents(actionerList, monitorProcess, actionParameters, exception, processesFound);
            }
            catch (System.Exception ex)
            {

            }
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorProcess monitorProcess, ActionParameters actionParameters, Exception exception, List<Process> processesFound)
        {
            foreach (EventItem eventItem in monitorProcess.EventItems)
            {
                bool meetsCondition = false;

                switch (eventItem.EventCondition.Source)
                {
                    case EventConditionSource.Exception:
                        meetsCondition = (exception != null);
                        break;
                    case EventConditionSource.NoException:
                        meetsCondition = (exception == null);
                        break;
                    case EventConditionSource.ProcessRunning:
                        meetsCondition = (processesFound.Count > 0);
                        break;
                    case EventConditionSource.ProcessNotRunning:
                        meetsCondition = (processesFound.Count == 0);
                        break;
                }

                /*
                if (eventItem.EventCondition.Source.Equals("OnException"))
                {
                    meetsCondition = (exception != null);
                }
                else if (eventItem.EventCondition.Source.Equals("OnNoException"))
                {
                    meetsCondition = (exception == null);
                }
                else if (eventItem.EventCondition.Source.Equals("OnProcessRunning"))
                {
                    meetsCondition = (processesFound.Count > 0);
                }
                else if (eventItem.EventCondition.Source.Equals("OnProcessNotRunning"))
                {
                    meetsCondition = (processesFound.Count == 0);
                }
                */

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
            return monitorItem is MonitorProcess;
        }

        private void DoAction(List<IActioner> actionerList, MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            foreach (IActioner actioner in actionerList)
            {
                if (actioner.CanAction(actionItem))
                {
                    actioner.DoAction(monitorItem, actionItem, actionParameters);
                    break;
                }
            }
        }
    }
}
