﻿using CFMonitor.Enums;
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

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem is ActionURL;
        }

        private static HttpWebRequest CreateWebRequest(ActionURL actionURL)
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
