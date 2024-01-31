using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace CFMonitor
{
    /// <summary>
    /// Checks REST web service
    /// </summary>
    public class CheckerREST : IChecker
    {
        public void Check(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorREST monitorREST = (MonitorREST)monitorItem;
            Exception exception = null;
            string result = "";
            HttpWebRequest webRequest = null;
            HttpWebResponse webResponse = null;
            ActionParameters actionParameters = new CFMonitor.ActionParameters();

            try
            {
                webRequest = CreateWebRequest(monitorREST);
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
                CheckEvents(actionerList, monitorREST, actionParameters, exception, webRequest, webResponse);
            }
            catch (Exception ex)
            {

            }
        }

        private HttpWebRequest CreateWebRequest(MonitorREST monitorREST)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(monitorREST.URL);
            webRequest.Method = "GET";            
            return webRequest;
        }
       
        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorREST;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorREST monitorREST, ActionParameters actionParameters, Exception exception, HttpWebRequest request, HttpWebResponse response)
        {
            int webExceptionStatus = -1;
            if (exception is WebException)
            {
                WebException webException = (WebException)exception;
                webExceptionStatus = Convert.ToInt32(webException.Status);
            }

            foreach (EventItem eventItem in monitorREST.EventItems)
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
                        DoAction(actionerList, monitorREST, actionItem, actionParameters);
                    }
                }
            }
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
