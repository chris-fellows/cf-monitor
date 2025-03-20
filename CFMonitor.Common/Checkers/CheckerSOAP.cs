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
using System.Xml;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks SOAP web service
    /// </summary>
    public class CheckerSOAP : IChecker
    {
        public string Name => "SOAP";

        public CheckerTypes CheckerType => CheckerTypes.SOAP;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            //MonitorSOAP monitorSOAP = (MonitorSOAP)monitorItem;
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

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.SOAP;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorItem monitorSOAP, ActionParameters actionParameters, Exception exception, HttpWebRequest request, HttpWebResponse response)
        {
            int webExceptionStatus = -1;
            if (exception is WebException)
            {
                WebException webException = (WebException)exception;
                webExceptionStatus = Convert.ToInt32(webException.Status);
            }

            foreach (EventItem eventItem in monitorSOAP.EventItems)
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
                        DoAction(actionerList, monitorSOAP, actionItem, actionParameters);
                    }
                }
            }
        }

        private HttpWebRequest CreateWebRequest(MonitorItem monitorSOAP)
        {
            var urlParam = monitorSOAP.Parameters.First(p => p.SystemValueType == SystemValueTypes.MIP_SOAPURL);
            var xmlParam = monitorSOAP.Parameters.First(p => p.SystemValueType == SystemValueTypes.MIP_SOAPXML);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlParam.Value);
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";

            // Set SOAP envelope
            XmlDocument envelopeDocument = new XmlDocument();
            envelopeDocument.LoadXml(xmlParam.Value);
            using (Stream stream = webRequest.GetRequestStream())
            {
                envelopeDocument.Save(stream);
            }
            return webRequest;
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
