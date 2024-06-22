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
            MonitorURL monitorURL = (MonitorURL)monitorItem;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Exception exception = null;
            ActionParameters actionParameters = new ActionParameters();

            try
            {
                // Create request
                request = CreateWebRequest(monitorURL);

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

        private HttpWebRequest CreateWebRequest(MonitorURL monitorURL)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(monitorURL.URL);
            request.Method = monitorURL.Method;
            request.KeepAlive = true;            
            if (!String.IsNullOrEmpty(monitorURL.ProxyName))
            {
                WebProxy proxy = new WebProxy(monitorURL.ProxyName, monitorURL.ProxyPort);
                if (!String.IsNullOrEmpty(monitorURL.UserName))
                {
                    proxy.Credentials = new NetworkCredential(monitorURL.UserName, monitorURL.Password);
                }
                request.Proxy = proxy;
            }
            return request;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorURL monitorURL, ActionParameters actionParameters, Exception exception, HttpWebRequest request, HttpWebResponse response)
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
            return monitorItem is MonitorURL;
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
