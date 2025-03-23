using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks memory
    /// </summary>
    public class CheckerMemory : CheckerBase, IChecker
    {        
        public CheckerMemory(IAuditEventFactory auditEventFactory, 
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService, 
            IEventItemService eventItemService,
                ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, systemValueTypeService)
        {
            
        }

        public string Name => "Memory";

        //public CheckerTypes CheckerType => CheckerTypes.Memory;

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

                //MonitorMemory monitorMemory = (MonitorMemory)monitorItem;
                Exception exception = null;
                var actionItemParameters = new List<ActionItemParameter>();

                try
                {


                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                try
                {
                    foreach (var eventItem in eventItems)
                    {
                        if (IsEventValid(eventItem, monitorItem, actionItemParameters, exception))
                        {
                            monitorItemOutput.EventItemIdsForAction.Add(eventItem.Id);
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                return monitorItemOutput;
            });
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.Memory;
        }

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorMemory, List<ActionItemParameter> actionItemParameters, Exception exception)
        {
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                }

            //if (meetsCondition)
            //{
            //    foreach (ActionItem actionItem in eventItem.ActionItems)
            //    {
            //        await ExecuteActionAsync(actionerList, monitorMemory, actionItem, actionParameters);
            //    }
            //}         

            return meetsCondition;
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
