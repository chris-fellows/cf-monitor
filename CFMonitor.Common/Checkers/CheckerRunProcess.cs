using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks result of running a process (Checks exit code)
    /// </summary>
    public class CheckerRunProcess : IChecker
    {
        public string Name => "Run Process";

        public CheckerTypes CheckerType => CheckerTypes.RunProcess;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorActiveProcess monitorProcess = (MonitorActiveProcess)monitorItem;
            Exception exception = null;
            ActionParameters actionParameters = new ActionParameters();
            int? exitCode = null;

            try
            {
           
            }
            catch (System.Exception ex)
            {
                exception = ex;
            }

            try
            {
                // Check events
                actionParameters.Values.Add("Body", "Error checking service");
                CheckEvents(actionerList, monitorProcess, actionParameters, exception, exitCode);
            }
            catch (System.Exception ex)
            {

            }

            return Task.CompletedTask;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorActiveProcess monitorProcess, ActionParameters actionParameters, Exception exception, int? exitCode)
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
                    case EventConditionSource.RunProcessExitCodeReturned:
                        meetsCondition = (eventItem.EventCondition.Evaluate(exitCode));
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
            return monitorItem is MonitorRunProcess;
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
