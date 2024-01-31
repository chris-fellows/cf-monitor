using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace CFMonitor
{
    /// <summary>
    /// Checks opening URL
    /// </summary>
    public class CheckerURL : IChecker
    {
        public void Check(MonitorItem monitorItem, List<IActioner> actionerList)
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
                actionParameters.Values.Add("Body", "Error checking URL");
                CheckEvents(actionerList, monitorURL, actionParameters, exception, request, response);
            }
            catch(System.Exception ex)
            {

            }
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
                if (eventItem.EventCondition.Source.Equals("OnException"))
                {                 
                    meetsCondition = (exception != null);
                }
                else if (eventItem.EventCondition.Source.Equals("OnNoException"))
                {                    
                    meetsCondition = (exception == null);
                }
                else if (eventItem.EventCondition.Source.Equals("Response.StatusCode"))
                {
                    // Check HTTP Status
                    meetsCondition = eventItem.EventCondition.Evaluate(response.StatusCode);
                }
                else if (eventItem.EventCondition.Source.Equals("WebException.Status"))
                {
                    meetsCondition = eventItem.EventCondition.Evaluate(webExceptionStatus);
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
                if (actioner.CanAction(actionItem))
                {
                    actioner.DoAction(monitorItem, actionItem, actionParameters);
                    break;
                }
            }
        }
    }
}
