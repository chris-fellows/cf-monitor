using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.Security;
using System.IO;

namespace CFMonitor
{
    public class CheckerSMTP : IChecker
    {
        public void Check(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorSMTP monitorSMTP = (MonitorSMTP)monitorItem;
            Exception exception = null;
            ActionParameters actionParameters = new CFMonitor.ActionParameters();

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
                if (eventItem.EventCondition.Source.Equals("OnException"))
                {
                    meetsCondition = (exception != null);
                }
                else if (eventItem.EventCondition.Source.Equals("OnNoException"))
                {
                    meetsCondition = (exception == null);
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
                if (actioner.CanAction(actionItem))
                {
                    actioner.DoAction(monitorItem, actionItem, actionParameters);
                    break;
                }
            }
        }
    }
}
