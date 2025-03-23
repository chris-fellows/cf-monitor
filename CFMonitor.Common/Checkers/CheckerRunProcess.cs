using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks result of running a process (Checks exit code)
    /// </summary>
    public class CheckerRunProcess : CheckerBase, IChecker
    {
        
        public CheckerRunProcess(IAuditEventFactory auditEventFactory, 
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService, 
            IEventItemService eventItemService,
            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, systemValueTypeService)
        {
            
        }

        public string Name => "Run Process";

        //public CheckerTypes CheckerType => CheckerTypes.RunProcess;

        public Task<MonitorItemOutput> CheckAsync(MonitorAgent monitorAgent, MonitorItem monitorItem, bool testMode)
        {
            return Task.Factory.StartNew(() =>
            {
                var monitorItemOutput = new MonitorItemOutput();

                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return monitorItemOutput;
                }

                //MonitorActiveProcess monitorProcess = (MonitorActiveProcess)monitorItem;
                Exception exception = null;
                var actionItemParameters = new List<ActionItemParameter>();
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
                    //actionParameters.Values.Add(ActionParameterTypes.Body, "Error running process");
                    foreach (var eventItem in eventItems)
                    {
                        if (IsEventValid(eventItem, monitorItem, actionItemParameters, exception, exitCode))
                        {
                            monitorItemOutput.EventItemIdsForAction.Add(eventItem.Id);
                        }
                    }
                }
                catch (System.Exception ex)
                {

                }

                return monitorItemOutput;
            });
        }

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorProcess, List<ActionItemParameter> actionItemParameters, Exception exception, int? exitCode)
        {            
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                    case SystemValueTypes.ECS_RunProcessExitCode:
                        meetsCondition = eventItem.EventCondition.IsValid(exitCode);
                        break;                 
                }

            //if (meetsCondition)
            //{
            //    foreach (ActionItem actionItem in eventItem.ActionItems)
            //    {
            //        await ExecuteActionAsync(actionerList, monitorProcess, actionItem, actionParameters);
            //    }
            //}
            //
            return meetsCondition;
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.RunProcess;                 
        }

        //private void DoAction(List<IActioner> actionerList, MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        //{
        //    foreach (IActioner actioner in actionerList)
        //    {
        //        if (actioner.CanExecute(actionItem))
        //        {
        //            actioner.ExecuteAsync(monitorItem, actionItem, actionParameters);
        //            break;
        //        }
        //    }
        //}
    }
}
