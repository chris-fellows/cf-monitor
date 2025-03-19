using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions opening a URL
    /// </summary>
    public class ActionerURL : IActioner
    {
        public string Name => "Opens URL";

        public ActionerTypes ActionerType => ActionerTypes.URL;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            //ActionURL actionURL = (ActionURL)actionItem;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Exception exception = null;

            try
            {
                // Create request
                request = CreateWebRequest(actionItem);

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

        private static HttpWebRequest CreateWebRequest(ActionItem actionURL)
        {
            var urlParam = actionURL.Parameters.First(p => p.SystemValueType == SystemValueTypes.AIP_URLURL);
            var methodParam = actionURL.Parameters.First(p => p.SystemValueType == SystemValueTypes.AIP_URLMethod);
            var usernameParam = actionURL.Parameters.FirstOrDefault(p => p.SystemValueType == SystemValueTypes.AIP_URLUsername);
            var passwordParam = actionURL.Parameters.FirstOrDefault(p => p.SystemValueType == SystemValueTypes.AIP_URLPassword);
            var proxyNameParam = actionURL.Parameters.FirstOrDefault(p => p.SystemValueType == SystemValueTypes.AIP_URLProxyName);
            var proxyPortParam = actionURL.Parameters.FirstOrDefault(p => p.SystemValueType == SystemValueTypes.AIP_URLProxyPort);

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
