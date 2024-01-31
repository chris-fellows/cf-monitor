using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace CFMonitor
{
    /// <summary>
    /// Actions opening a URL
    /// </summary>
    public class ActionerURL : IActioner
    {
        public void DoAction(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            ActionURL actionURL = (ActionURL)actionItem;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Exception exception = null;

            try
            {
                // Create request
                request = CreateWebRequest(actionURL);

                // Get response
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (System.Exception ex)
            {
                exception = ex;
            }
        }

        public bool CanAction(ActionItem actionItem)
        {
            return actionItem is ActionURL;
        }

        private HttpWebRequest CreateWebRequest(ActionURL actionURL)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(actionURL.URL);
            request.Method = actionURL.Method;
            request.KeepAlive = true;
            if (!String.IsNullOrEmpty(actionURL.ProxyName))
            {
                WebProxy proxy = new WebProxy(actionURL.ProxyName, actionURL.ProxyPort);
                if (!String.IsNullOrEmpty(actionURL.UserName))
                {
                    proxy.Credentials = new NetworkCredential(actionURL.UserName, actionURL.Password);
                }
                request.Proxy = proxy;
            }
            return request;
        }
    }
}
