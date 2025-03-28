using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFUtilities.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks REST web service
    /// </summary>
    public class CheckerREST : CheckerBase, IChecker
    {        
        public CheckerREST(IAuditEventFactory auditEventFactory, 
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService, 
                IEventItemService eventItemService,
                  IFileObjectService fileObjectService,
                IPlaceholderService placeholderService,
                ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, fileObjectService, placeholderService, systemValueTypeService)
        {
            
        }

        public string Name => "REST API";

        //public CheckerTypes CheckerType => CheckerTypes.REST;

        public Task<MonitorItemOutput> CheckAsync(MonitorAgent monitorAgent, MonitorItem monitorItem, CheckerConfig checkerConfig)
        {
            return Task.Factory.StartNew(() =>
            {
                SetPlaceholders(monitorAgent, monitorItem, checkerConfig);

                var monitorItemOutput = new MonitorItemOutput()
                {
                    Id = Guid.NewGuid().ToString(),
                    ActionItemParameters = new(),
                    CheckedDateTime = DateTime.UtcNow,
                    EventItemIdsForAction = new(),
                    MonitorAgentId = monitorAgent.Id,
                    MonitorItemId = monitorItem.Id,
                };

                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return monitorItemOutput;
                }

                var systemValueTypes = _systemValueTypeService.GetAll();

                //MonitorREST monitorREST = (MonitorREST)monitorItem;
                Exception exception = null;
                string result = "";
                HttpWebRequest webRequest = null;
                HttpWebResponse webResponse = null;
             
                try
                {
                    webRequest = CreateWebRequest(monitorItem, systemValueTypes);
                    webResponse = (HttpWebResponse)webRequest.GetResponse();

                    //using (WebResponse response = webRequest.GetResponse())
                    //{
                    using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                    }
                    //}
                }
                catch (Exception ex)
                {
                    exception = ex;

                    monitorItemOutput.ActionItemParameters.Add(new ActionItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIPC_ErrorMessage).Id,
                        Value = ex.Message
                    });
                }

                try
                {
                    if (webResponse != null)
                    {
                        monitorItemOutput.ActionItemParameters.Add(new ActionItemParameter()
                        {
                            SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIPC_Message).Id,
                            Value = $"Response status={webResponse.StatusCode}"
                        });
                    }

                    foreach (var eventItem in eventItems)
                    {
                        if (IsEventValid(eventItem, monitorItem, exception, webRequest, webResponse))
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

        private HttpWebRequest CreateWebRequest(MonitorItem monitorREST, List<SystemValueType> systemValueTypes)
        {
            var svtParam = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_RESTURL);            
            var urlParam = monitorREST.Parameters.First(p => p.SystemValueTypeId == svtParam.Id);
            var url = GetValueWithPlaceholdersReplaced(urlParam);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";            
            return webRequest;
        }
       
        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.REST;
        }

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorREST, Exception exception, HttpWebRequest request, HttpWebResponse response)
        {            
            int webExceptionStatus = -1;
            if (exception is WebException)
            {
                WebException webException = (WebException)exception;
                webExceptionStatus = Convert.ToInt32(webException.Status);
            }
            
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                    case SystemValueTypes.ECS_HTTPResponseStatusCode:
                        meetsCondition = eventItem.EventCondition.IsValid(response.StatusCode);
                        break;

                    /*
                    case EventConditionSources.HttpResponseStatusCode:
                        meetsCondition = eventItem.EventCondition.IsValid(response.StatusCode);
                        break;
                    case EventConditionSources.WebExceptionStatus:
                        meetsCondition = eventItem.EventCondition.IsValid(webExceptionStatus);
                        break;
                    */
                }

            //if (meetsCondition)
            //{
            //    foreach (ActionItem actionItem in eventItem.ActionItems)
            //    {
            //        await ExecuteActionAsync(actionerList, monitorREST, actionItem, actionParameters);
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
