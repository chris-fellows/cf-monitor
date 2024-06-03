using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks ping to server
    /// </summary>
    public class CheckerPing : IChecker
    {
        public string Name => "Ping";

        public CheckerTypes CheckerType => CheckerTypes.Ping;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorPing monitorPing = (MonitorPing)monitorItem;
            Exception exception = null;
            PingReply pingReply = null;
            ActionParameters actionParameters = new ActionParameters();

            try
            {
                // Check ping
                Ping ping = new Ping();
                PingOptions pingOptions = new PingOptions();                
                pingOptions.DontFragment = true;

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                pingReply = ping.Send(monitorPing.Server, timeout, buffer, pingOptions);
                /*
                if (reply.Status == IPStatus.Success)
                {                    
                    Console.WriteLine("Address: {0}", reply.Address.ToString());
                    Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                    Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                    Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                    Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
                }
                */
            }
            catch (System.Exception ex)
            {
                exception = ex;
            }

            try
            {
                // Check events
                CheckEvents(actionerList, monitorPing, actionParameters, exception, pingReply);
            }
            catch (System.Exception ex)
            {

            }

            return Task.CompletedTask;
        }        

        private void CheckEvents(List<IActioner> actionerList, MonitorPing monitorPing, ActionParameters actionParameters, Exception exception, PingReply pingReply)
        {
            foreach (EventItem eventItem in monitorPing.EventItems)
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
                    case EventConditionSource.PingReplyStatus:
                        meetsCondition = (pingReply != null && eventItem.EventCondition.Evaluate(pingReply.Status));
                        break;
                }

                /*
                if (eventItem.EventCondition.Source.Equals("OnException"))
                {
                    meetsCondition = (exception != null);
                }
                else if (eventItem.EventCondition.Source.Equals("OnNoException"))
                {
                    meetsCondition = (exception == null);
                }
                else if (eventItem.EventCondition.Source.Equals("Reply.Status"))
                {
                    meetsCondition = (pingReply != null && eventItem.EventCondition.Evaluate(pingReply.Status));
                }
                */

                if (meetsCondition)
                {
                    foreach(ActionItem actionItem in eventItem.ActionItems)
                    {
                        DoAction(actionerList, monitorPing, actionItem, actionParameters);
                    }
                }
            }
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorPing;
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
