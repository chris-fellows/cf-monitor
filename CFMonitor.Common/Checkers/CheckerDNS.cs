using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFUtilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks DNS
    /// </summary>
    public class CheckerDNS : CheckerBase, IChecker
    {        
        public CheckerDNS(IAuditEventFactory auditEventFactory, 
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService,
            IEventItemService eventItemService,
              IFileObjectService fileObjectService,
            IPlaceholderService placeholderService,
            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, fileObjectService, placeholderService, systemValueTypeService)
        {
     
        }

        public string Name => "DNS";

       // public CheckerTypes CheckerType => CheckerTypes.DNS;

        public Task<MonitorItemOutput> CheckAsync(MonitorAgent monitorAgent, MonitorItem monitorItem, CheckerConfig checkerConfig)
        {
            return Task.Factory.StartNew( () =>
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

                var svtHost = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_DNSHost);
                var hostParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtHost.Id);
                var host = GetValueWithPlaceholdersReplaced(hostParam);

                Exception exception = null;
                IPHostEntry hostEntry = null;
                var actionItemParameters = new List<ActionItemParameter>();

                try
                {
                    hostEntry = Dns.GetHostEntry(host);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                try
                {
                    foreach (var eventItem in eventItems)
                    {
                        if (IsEventValid(eventItem, monitorItem, actionItemParameters, exception, hostEntry))
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
            return monitorItem.MonitorItemType == MonitorItemTypes.DNS;
        }

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorDNS, List<ActionItemParameter> actionItemParameters, Exception exception, IPHostEntry hostEntry)
        {            
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                    case SystemValueTypes.ECS_DNSHostExists:
                        meetsCondition = eventItem.EventCondition.IsValid(hostEntry != null);
                        break;
                }


            //if (meetsCondition)
            //{
            //    foreach (ActionItem actionItem in eventItem.ActionItems)
            //    {
            //        await ExecuteActionAsync(actionerList, monitorDNS, actionItem, actionParameters);
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
