﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.IO;

namespace CFMonitor
{
    /// <summary>
    /// Checks SOAP web service
    /// </summary>
    public class CheckerSOAP : IChecker
    {
        public void Check(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorSOAP monitorSOAP = (MonitorSOAP)monitorItem;
            Exception exception = null;
            string result = "";
            HttpWebRequest webRequest = null;
            HttpWebResponse webResponse = null;
            ActionParameters actionParameters = new CFMonitor.ActionParameters();

            try
            {
                webRequest = CreateWebRequest(monitorSOAP);          
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
                CheckEvents(actionerList, monitorSOAP, actionParameters, exception, webRequest, webResponse);
            }
            catch (Exception ex)
            {

            }
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorSOAP;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorSOAP monitorSOAP, ActionParameters actionParameters, Exception exception, HttpWebRequest request, HttpWebResponse response)
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
                        DoAction(actionerList, monitorSOAP, actionItem, actionParameters);
                    }
                }
            }
        }

        private HttpWebRequest CreateWebRequest(MonitorSOAP monitorSOAP)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(monitorSOAP.URL);
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";

            // Set SOAP envelope
            XmlDocument envelopeDocument = new XmlDocument();
            envelopeDocument.LoadXml(monitorSOAP.XML);
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
                if (actioner.CanAction(actionItem))
                {
                    actioner.DoAction(monitorItem, actionItem, actionParameters);
                    break;
                }
            }
        }
    }
}
