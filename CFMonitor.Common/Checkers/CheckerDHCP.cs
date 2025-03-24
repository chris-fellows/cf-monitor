using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFUtilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks DHCP server
    /// </summary>
    public class CheckerDHCP : CheckerBase, IChecker
    {        
        public CheckerDHCP(IAuditEventFactory auditEventFactory, 
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService, 
            IEventItemService eventItemService,
            IPlaceholderService placeholderService,
            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, placeholderService, systemValueTypeService)
        {
            
        }

        public string Name => "DHCP";

        //public CheckerTypes CheckerType => CheckerTypes.DHCP;

        public Task<MonitorItemOutput> CheckAsync(MonitorAgent monitorAgent, MonitorItem monitorItem, CheckerConfig checkerConfig)
        {
            return Task.Factory.StartNew(() =>
            {
                SetPlaceholders(monitorAgent, monitorItem, checkerConfig);

                var monitorItemOutput = new MonitorItemOutput();

                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return monitorItemOutput;
                }

                var systemValueTypes = _systemValueTypeService.GetAll();

                //MonitorDHCP monitorDHCP = (MonitorDHCP)monitorItem;
                Exception exception = null;
                var actionItemParameters = new List<ActionItemParameter>();

                try
                {

                }
                catch (Exception ex)
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
            return monitorItem.MonitorItemType == MonitorItemTypes.DHCP;
        }

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorDHCP, List<ActionItemParameter> actionItemParameters, Exception exception)
        {            
                bool meetsCondition = false;
                switch(eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                }

            //if (meetsCondition)
            //{
            //    foreach (ActionItem actionItem in eventItem.ActionItems)
            //    {
            //        await ExecuteActionAsync(actionerList, monitorDHCP, actionItem, actionItemParameters);
            //    }
            //}         

            return meetsCondition;
        }

        //private void DoAction(List<IActioner> actionerList, MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> actionItemParameters)
        //{
        //    foreach (IActioner actioner in actionerList)
        //    {
        //        if (actioner.CanExecute(actionItem))
        //        {
        //            actioner.ExecuteAsync(monitorItem, actionItem, actionItemParameters);
        //            break;
        //        }
        //    }
        //}
    }
}
