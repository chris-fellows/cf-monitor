using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks REST web service
    /// </summary>
    public class CheckerREST : IChecker
    {
        public string Name => "REST API";

        public CheckerTypes CheckerType => CheckerTypes.REST;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            //MonitorREST monitorREST = (MonitorREST)monitorItem;
            Exception exception = null;
            string result = "";
            HttpWebRequest webRequest = null;
            HttpWebResponse webResponse = null;
            ActionParameters actionParameters = new ActionParameters();

            try
            {
                webRequest = CreateWebRequest(monitorItem);
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
                CheckEvents(actionerList, monitorItem, actionParameters, exception, webRequest, webResponse);
            }
            catch (Exception ex)
            {

            }

            return Task.CompletedTask;
        }

        private HttpWebRequest CreateWebRequest(MonitorItem monitorREST)
        {
            var urlParam = monitorREST.Parameters.First(p => p.SystemValueType == SystemValueTypes.MIP_RESTURL);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlParam.Value);
            webRequest.Method = "GET";            
            return webRequest;
        }
       
        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.REST;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorItem monitorREST, ActionParameters actionParameters, Exception exception, HttpWebRequest request, HttpWebResponse response)
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

                switch (eventItem.EventCondition.Source)
                {
                    case EventConditionSource.Exception:
                        meetsCondition = (exception != null);
                        break;
                    case EventConditionSource.NoException:
                        meetsCondition = (exception == null);
                        break;
                    case EventConditionSource.HttpResponseStatusCode:
                        meetsCondition = eventItem.EventCondition.IsValid(response.StatusCode);
                        break;
                    case EventConditionSource.WebExceptionStatus:
                        meetsCondition = eventItem.EventCondition.IsValid(webExceptionStatus);
                        break;
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
                if (actioner.CanExecute(actionItem))
                {
                    actioner.ExecuteAsync(monitorItem, actionItem, actionParameters);
                    break;
                }
            }
        }
    }
}
