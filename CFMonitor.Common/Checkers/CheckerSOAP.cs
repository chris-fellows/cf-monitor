using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
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
    public class CheckerSOAP : CheckerBase, IChecker
    {        
        public CheckerSOAP(IEventItemService eventItemService,
                        ISystemValueTypeService systemValueTypeService) : base(eventItemService, systemValueTypeService)
        {
     
        }

        public string Name => "SOAP";

       // public CheckerTypes CheckerType => CheckerTypes.SOAP;

        public async Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
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

                //MonitorSOAP monitorSOAP = (MonitorSOAP)monitorItem;
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

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.SOAP;
        }

        private async Task CheckEventAsync(EventItem eventItem, List<IActioner> actionerList, MonitorItem monitorSOAP, ActionParameters actionParameters, Exception exception, HttpWebRequest request, HttpWebResponse response)
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
                        await ExecuteActionAsync(actionerList, monitorSOAP, actionItem, actionParameters);
                    }
                }            
        }

        private HttpWebRequest CreateWebRequest(MonitorItem monitorSOAP, List<SystemValueType> systemValueTypes)
        {
            var svtUrl = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_SOAPURL);
            var urlParam = monitorSOAP.Parameters.First(p => p.SystemValueTypeId == svtUrl.Id);

            var svtXml = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_SOAPXML);
            var xmlParam = monitorSOAP.Parameters.First(p => p.SystemValueTypeId == svtXml.Id);

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
