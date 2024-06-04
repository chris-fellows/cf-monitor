using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks SMTP server
    /// </summary>
    public class CheckerSMTP : IChecker
    {
        public string Name => "SMTP";

        public CheckerTypes CheckerType => CheckerTypes.SMTP;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorSMTP monitorSMTP = (MonitorSMTP)monitorItem;
            Exception exception = null;
            ActionParameters actionParameters = new ActionParameters();

            try
            {
                using (var client = new TcpClient())
                {
                          
                    //var server = "smtp.gmail.com";
                    //var port = 465;
                    client.Connect(monitorSMTP.Server, monitorSMTP.Port);
                    // As GMail requires SSL we should use SslStream
                    // If your SMTP server doesn't support SSL you can
                    // work directly with the underlying stream
                    using (var stream = client.GetStream())
                    using (var sslStream = new SslStream(stream))
                    {
                        sslStream.AuthenticateAsClient(monitorSMTP.Server);
                        using (var writer = new StreamWriter(sslStream))
                        using (var reader = new StreamReader(sslStream))
                        {
                            writer.WriteLine("EHLO " + monitorSMTP.Server);
                            writer.Flush();
                            Console.WriteLine(reader.ReadLine());
                            // GMail responds with: 220 mx.google.com ESMTP
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            try
            {
                CheckEvents(actionerList, monitorSMTP, actionParameters, exception);
            }
            catch (Exception ex)
            {

            }

            return Task.CompletedTask;
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorSMTP;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorSMTP monitorSMTP, ActionParameters actionParameters, Exception exception)
        {
            foreach (EventItem eventItem in monitorSMTP.EventItems)
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
                }

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        DoAction(actionerList, monitorSMTP, actionItem, actionParameters);
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
