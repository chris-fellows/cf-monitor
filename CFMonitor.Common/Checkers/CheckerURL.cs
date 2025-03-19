using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks opening URL (HTTP/HTTPS)
    /// </summary>
    public class CheckerURL : IChecker
    {
        public string Name => "URL";

        public CheckerTypes CheckerType => CheckerTypes.URL;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
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
                CheckEvents(actionerList, monitorURL, actionParameters, exception, request, response);
            }
            catch(System.Exception ex)
            {

            }

            return Task.CompletedTask;
        }

        private HttpWebRequest CreateWebRequest(MonitorItem monitorURL)
        {
            var urlParam = monitorURL.Parameters.First(p => p.SystemValueType == SystemValueTypes.MIP_URLURL);
            var methodParam = monitorURL.Parameters.First(p => p.SystemValueType == SystemValueTypes.MIP_URLMethod);
            var usernameParam = monitorURL.Parameters.FirstOrDefault(p => p.SystemValueType == SystemValueTypes.MIP_URLUsername);
            var passwordParam = monitorURL.Parameters.FirstOrDefault(p => p.SystemValueType == SystemValueTypes.MIP_URLPassword);
            var proxyNameParam = monitorURL.Parameters.FirstOrDefault(p => p.SystemValueType == SystemValueTypes.MIP_URLProxyName);
            var proxyPortParam = monitorURL.Parameters.FirstOrDefault(p => p.SystemValueType == SystemValueTypes.MIP_URLProxyPort);

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

        private void CheckEvents(List<IActioner> actionerList, MonitorItem monitorURL, ActionParameters actionParameters, Exception exception, HttpWebRequest request, HttpWebResponse response)
        {
            int webExceptionStatus = -1;
            if (exception is WebException)
            {
                WebException webException = (WebException)exception;
                webExceptionStatus = Convert.ToInt32(webException.Status);                
            }

            foreach (EventItem eventItem in monitorURL.EventItems)
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
                    case EventConditionSource.HttpResponseStatusCode:
                        meetsCondition = eventItem.EventCondition.Evaluate(response.StatusCode);
                        break;
                    case EventConditionSource.WebExceptionStatus:
                        meetsCondition = eventItem.EventCondition.Evaluate(webExceptionStatus);
                        break;
                }          

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        DoAction(actionerList, monitorURL, actionItem, actionParameters);
                    }
                }
            }
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.URL;                 
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
