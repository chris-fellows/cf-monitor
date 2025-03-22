﻿using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks opening URL (HTTP/HTTPS)
    /// </summary>
    public class CheckerURL : CheckerBase, IChecker
    {        
        public CheckerURL(IEventItemService eventItemService,
                            ISystemValueTypeService systemValueTypeService) : base(eventItemService, systemValueTypeService)
        {
     
        }

        public string Name => "URL";

        //public CheckerTypes CheckerType => CheckerTypes.URL;

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

                //MonitorURL monitorURL = (MonitorURL)monitorItem;

                HttpWebRequest request = null;
                HttpWebResponse response = null;
                Exception exception = null;
                ActionParameters actionParameters = new ActionParameters();

                try
                {
                    // Create request
                    request = CreateWebRequest(monitorItem);

                    // Get response
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (System.Exception ex)
                {
                    exception = ex;
                }

                try
                {
                    // Check events
                    actionParameters.Values.Add(ActionParameterTypes.Body, "Error checking URL");
                    foreach (var eventItem in eventItems)
                    {
                        await CheckEventAsync(eventItem, actionerList, monitorItem, actionParameters, exception, request, response);
                    }
                }
                catch (System.Exception ex)
                {

                }
            });
        }

        private HttpWebRequest CreateWebRequest(MonitorItem monitorURL)
        {
            var systemValueTypes = _systemValueTypeService.GetAll();

            var svtParam = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLURL);
            var urlParam = monitorURL.Parameters.First(p => p.SystemValueTypeId == svtParam.Id);

            var svtMethod = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLMethod);
            var methodParam = monitorURL.Parameters.First(p => p.SystemValueTypeId == svtMethod.Id);

            var svtUsername = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLUsername);
            var usernameParam = monitorURL.Parameters.FirstOrDefault(p => p.SystemValueTypeId == svtUsername.Id);

            var svtPassword = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLPassword);
            var passwordParam = monitorURL.Parameters.FirstOrDefault(p => p.SystemValueTypeId == svtPassword.Id);

            var svtProxyName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLProxyName);
            var proxyNameParam = monitorURL.Parameters.FirstOrDefault(p => p.SystemValueTypeId == svtProxyName.Id);

            var svtProxyPort = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLProxyPort);
            var proxyPortParam = monitorURL.Parameters.FirstOrDefault(p => p.SystemValueTypeId == svtProxyPort.Id);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlParam.Value);
            request.Method = methodParam.Value;
            request.KeepAlive = true;            
            if (proxyNameParam != null && !String.IsNullOrEmpty(proxyNameParam.Value))
            {
                WebProxy proxy = new WebProxy(proxyNameParam.Value, Convert.ToInt32(proxyPortParam.Value));
                //if (!String.IsNullOrEmpty(monitorURL.UserName))
                if (usernameParam != null && !String.IsNullOrWhiteSpace(usernameParam.Value))
                {
                    proxy.Credentials = new NetworkCredential(usernameParam.Value, passwordParam.Value);
                }
                request.Proxy = proxy;
            }
            return request;
        }

        private async Task CheckEventAsync(EventItem eventItem, List<IActioner> actionerList, MonitorItem monitorURL, ActionParameters actionParameters, Exception exception, HttpWebRequest request, HttpWebResponse response)
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
                        await ExecuteActionAsync(actionerList, monitorURL, actionItem, actionParameters);
                    }
                }
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.URL;                 
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
