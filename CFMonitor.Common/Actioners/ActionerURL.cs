using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions opening a URL
    /// </summary>
    public class ActionerURL : ActionerBase, IActioner
    {        
        public ActionerURL(IAuditEventFactory auditEventFactory, 
                            IAuditEventService auditEventService,
                            IAuditEventTypeService auditEventTypeService, 
                            ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, systemValueTypeService)
        {
     
        }

        public string Name => "Opens URL";

        //public ActionerTypes ActionerType => ActionerTypes.URL;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> parameters)
        {
            //ActionURL actionURL = (ActionURL)actionItem;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Exception exception = null;

            var systemValueTypes = _systemValueTypeService.GetAll();

            try
            {
                // Create request
                request = CreateWebRequest(actionItem, systemValueTypes);

                // Get response
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (System.Exception ex)
            {
                exception = ex;
            }

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem.ActionerType == ActionerTypes.URL;
        }

        private static HttpWebRequest CreateWebRequest(ActionItem actionURL, List<SystemValueType> systemValueTypes)
        {
            var urlParam = actionURL.Parameters.First(p => p.SystemValueTypeId == systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIP_URLURL).Id);
            var methodParam = actionURL.Parameters.First(p => p.SystemValueTypeId == systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIP_URLMethod).Id);
            var usernameParam = actionURL.Parameters.FirstOrDefault(p => p.SystemValueTypeId == systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIP_URLUsername).Id);
            var passwordParam = actionURL.Parameters.FirstOrDefault(p => p.SystemValueTypeId == systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIP_URLPassword).Id);
            var proxyNameParam = actionURL.Parameters.FirstOrDefault(p => p.SystemValueTypeId == systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIP_URLProxyName).Id);
            var proxyPortParam = actionURL.Parameters.FirstOrDefault(p => p.SystemValueTypeId == systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIP_URLProxyPort).Id);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlParam.Value);
            request.Method = methodParam.Value;
            request.KeepAlive = true;
            if (proxyNameParam != null && !String.IsNullOrEmpty(proxyNameParam.Value))
            {
                WebProxy proxy = new WebProxy(proxyNameParam.Value, Convert.ToInt32(proxyPortParam.Value));
                if (usernameParam != null && !String.IsNullOrEmpty(usernameParam.Value))
                {
                    proxy.Credentials = new NetworkCredential(usernameParam.Value, passwordParam.Value);
                }
                request.Proxy = proxy;
            }
            return request;
        }
    }
}
