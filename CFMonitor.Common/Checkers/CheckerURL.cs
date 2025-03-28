using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFUtilities.Interfaces;
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
        public CheckerURL(IAuditEventFactory auditEventFactory,
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService, 
            IEventItemService eventItemService,
            IFileObjectService fileObjectService,
            IPlaceholderService placeholderService,
            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, fileObjectService, placeholderService, systemValueTypeService)
        {
     
        }

        public string Name => "URL";

        //public CheckerTypes CheckerType => CheckerTypes.URL;

        public Task<MonitorItemOutput> CheckAsync(MonitorAgent monitorAgent, MonitorItem monitorItem, CheckerConfig checkerConfig)
        {
            return Task.Factory.StartNew(() =>
            {
                SetPlaceholders(monitorAgent, monitorItem, checkerConfig);

                var monitorItemOutput = new MonitorItemOutput()
                {
                    Id = Guid.NewGuid().ToString(),
                    ActionItemParameters = new(),
                    CheckedDateTime = DateTime.UtcNow,
                    EventItemIdsForAction = new(),
                    MonitorAgentId = monitorAgent.Id,
                    MonitorItemId = monitorItem.Id,
                };

                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return monitorItemOutput;
                }

                var systemValueTypes = _systemValueTypeService.GetAll();

                HttpWebRequest request = null;
                HttpWebResponse response = null;
                Exception exception = null;                

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

                    monitorItemOutput.ActionItemParameters.Add(new ActionItemParameter()
                    {
                        SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIPC_ErrorMessage).Id,
                        Value = ex.Message
                    });
                }

                try
                {
                    if (response != null)
                    {
                        monitorItemOutput.ActionItemParameters.Add(new ActionItemParameter()
                        {
                            SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIPC_Message).Id,
                            Value = $"Response status={response.StatusCode}"
                        });
                    }

                    // Check events
                    foreach (var eventItem in eventItems)
                    {
                        if (IsEventValid(eventItem, monitorItem, exception, request, response))
                        {
                            monitorItemOutput.EventItemIdsForAction.Add(eventItem.Id);
                        }
                    }
                }
                catch (System.Exception ex)
                {

                }

                return monitorItemOutput;
            });
        }

        private HttpWebRequest CreateWebRequest(MonitorItem monitorURL)
        {
            var systemValueTypes = _systemValueTypeService.GetAll();

            var svtParam = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLURL);
            var urlParam = monitorURL.Parameters.First(p => p.SystemValueTypeId == svtParam.Id);
            var url = GetValueWithPlaceholdersReplaced(urlParam);

            var svtMethod = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLMethod);
            var methodParam = monitorURL.Parameters.First(p => p.SystemValueTypeId == svtMethod.Id);
            var method = GetValueWithPlaceholdersReplaced(methodParam);

            var svtUsername = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLUsername);
            var usernameParam = monitorURL.Parameters.FirstOrDefault(p => p.SystemValueTypeId == svtUsername.Id);
            var username = GetValueWithPlaceholdersReplaced(usernameParam);

            var svtPassword = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLPassword);
            var passwordParam = monitorURL.Parameters.FirstOrDefault(p => p.SystemValueTypeId == svtPassword.Id);
            var password = GetValueWithPlaceholdersReplaced(passwordParam);

            var svtProxyName = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLProxyName);
            var proxyNameParam = monitorURL.Parameters.FirstOrDefault(p => p.SystemValueTypeId == svtProxyName.Id);
            var proxyName = GetValueWithPlaceholdersReplaced(proxyNameParam);

            var svtProxyPort = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_URLProxyPort);
            var proxyPortParam = monitorURL.Parameters.FirstOrDefault(p => p.SystemValueTypeId == svtProxyPort.Id);
            var proxyPort = GetValueWithPlaceholdersReplaced(proxyPortParam);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlParam.Value);
            request.Method = methodParam.Value;
            request.KeepAlive = true;            
            if (proxyNameParam != null && !String.IsNullOrEmpty(proxyName))
            {
                WebProxy proxy = new WebProxy(proxyName, Convert.ToInt32(proxyPort));
                //if (!String.IsNullOrEmpty(monitorURL.UserName))
                if (usernameParam != null && !String.IsNullOrWhiteSpace(username))
                {
                    proxy.Credentials = new NetworkCredential(username, password);
                }
                request.Proxy = proxy;
            }
            return request;
        }

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorURL, Exception exception, HttpWebRequest request, HttpWebResponse response)
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
                }
            
            return meetsCondition;
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
