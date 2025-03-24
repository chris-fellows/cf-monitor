using CFMonitor.Enums;
using CFMonitor.Exceptions;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFUtilities.Interfaces;
using CFUtilities.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks result of local time within tolerance of remote time (NIST, NTP, HTTP)
    /// </summary>
    public class CheckerTime : CheckerBase, IChecker
    {
        
        public CheckerTime(IAuditEventFactory auditEventFactory, 
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService, 
            IEventItemService eventItemService,
            IPlaceholderService  placeholderService,
            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, placeholderService,  systemValueTypeService)
        {
            
        }

        public string Name => "Time";

        //public CheckerTypes CheckerType => CheckerTypes.NTP;

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

                //MonitorNTP monitorNTP = (MonitorNTP)monitorItem;
                Exception exception = null;
                var actionItemParameters = new List<ActionItemParameter>();

                var systemValueTypes = _systemValueTypeService.GetAll();

                var svtServerType = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_TimeServerType);
                var serverTypeParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtServerType.Id);
                var serverType = GetValueWithPlaceholdersReplaced(serverTypeParam);

                var svtServer = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_TimeServer);
                var serverParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtServer.Id);
                var server = GetValueWithPlaceholdersReplaced(serverParam);

                var svtMaxToleranceSecs = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_TimeMaxToleranceSecs);
                var maxToleranceSecsParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtMaxToleranceSecs.Id);
                var maxToleranceSecs = Convert.ToInt32(GetValueWithPlaceholdersReplaced(maxToleranceSecsParam));

                var isTimeInTolerance = false;      // Default
                try
                {
                    var timeRemote = serverType switch
                    {
                        "NIST" => TimeUtilities.GetNISTTimeAsync(server).Result,
                        "NTP" => TimeUtilities.GetNTPTimeAsync(server).Result,
                        "HTTP" => TimeUtilities.GetHTTPTimeAsync(server).Result,
                        _ => throw new CheckerException($"Invalid server type {serverType}")
                    };
                
                    // Get local time
                    var timeLocal = DateTimeOffset.UtcNow;

                    // Check if in tolernace
                    isTimeInTolerance = Math.Abs((timeRemote.Value - timeLocal).TotalSeconds) <= maxToleranceSecs;
                }
                catch (System.Exception ex)
                {
                    exception = ex;
                }

                try
                {
                    // Check events
                    //actionParameters.Values.Add(ActionParameterTypes.Body, "Error checking NTP time");
                    foreach (var eventItem in eventItems)
                    {
                        if (IsEventValid(eventItem,  monitorItem, actionItemParameters, exception, isTimeInTolerance))
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

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorNTP, List<ActionItemParameter> actionItemParameters, Exception exception, bool isTimeInTolerance)
        {
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                    case SystemValueTypes.ECS_NTPTimeInTolerance:
                        meetsCondition = eventItem.EventCondition.IsValid(isTimeInTolerance);
                        break;
                }

            //if (meetsCondition)
            //{
            //    foreach (ActionItem actionItem in eventItem.ActionItems)
            //    {
            //        await ExecuteActionAsync(actionerList, monitorNTP, actionItem, actionParameters);
            //    }
            //}
            //

            return meetsCondition;
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.NTP;
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
