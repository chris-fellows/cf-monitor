using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
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
        public CheckerDNS(IEventItemService eventItemService,
            ISystemValueTypeService systemValueTypeService) : base(eventItemService, systemValueTypeService)
        {
     
        }

        public string Name => "DNS";

       // public CheckerTypes CheckerType => CheckerTypes.DNS;

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

                var svtHost = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_DNSHost);
                var hostParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtHost.Id);

                Exception exception = null;
                IPHostEntry hostEntry = null;
                ActionParameters actionParameters = new ActionParameters();

                try
                {
                    hostEntry = Dns.GetHostEntry(hostParam.Value);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                try
                {
                    foreach (var eventItem in eventItems)
                    {
                        await CheckEventAsync(eventItem, actionerList, monitorItem, actionParameters, exception, hostEntry);
                    }
                }
                catch (Exception ex)
                {

                }

                return Task.CompletedTask;
            });
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.DNS;
        }

        private async Task CheckEventAsync(EventItem eventItem, List<IActioner> actionerList, MonitorItem monitorDNS, ActionParameters actionParameters, Exception exception, IPHostEntry hostEntry)
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
         
                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        await ExecuteActionAsync(actionerList, monitorDNS, actionItem, actionParameters);
                    }
                }            
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
