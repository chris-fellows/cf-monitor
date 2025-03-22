using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks REST web service
    /// </summary>
    public class CheckerREST : CheckerBase, IChecker
    {        
        public CheckerREST(IEventItemService eventItemService,
                ISystemValueTypeService systemValueTypeService) : base(eventItemService, systemValueTypeService)
        {
            
        }

        public string Name => "REST API";

        //public CheckerTypes CheckerType => CheckerTypes.REST;

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

                //MonitorREST monitorREST = (MonitorREST)monitorItem;
                Exception exception = null;
                string result = "";
                HttpWebRequest webRequest = null;
                HttpWebResponse webResponse = null;
                ActionParameters actionParameters = new ActionParameters();

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
                }

                try
                {
                    foreach (var eventItem in eventItems)
                    {
                        await CheckEventAsync(eventItem, actionerList, monitorItem, actionParameters, exception, webRequest, webResponse);
                    }
                }
                catch (Exception ex)
                {

                }

            });
        }

        private HttpWebRequest CreateWebRequest(MonitorItem monitorREST, List<SystemValueType> systemValueTypes)
        {
            var svtParam = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_RESTURL);
            var urlParam = monitorREST.Parameters.First(p => p.SystemValueTypeId == svtParam.Id);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlParam.Value);
            webRequest.Method = "GET";            
            return webRequest;
        }
       
        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.REST;
        }

        private async Task CheckEventAsync(EventItem eventItem, List<IActioner> actionerList, MonitorItem monitorREST, ActionParameters actionParameters, Exception exception, HttpWebRequest request, HttpWebResponse response)
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
              
                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        await ExecuteActionAsync(actionerList, monitorREST, actionItem, actionParameters);
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
