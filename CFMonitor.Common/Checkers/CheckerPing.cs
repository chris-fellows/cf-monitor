using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
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
    public class CheckerPing : CheckerBase, IChecker
    {        
        public CheckerPing(IAuditEventFactory auditEventFactory, 
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService, 
                IEventItemService eventItemService,
                    ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, systemValueTypeService)
        {
     
        }

        public string Name => "Ping";

        //public CheckerTypes CheckerType => CheckerTypes.Ping;

        public Task<MonitorItemOutput> CheckAsync(MonitorAgent monitorAgent, MonitorItem monitorItem,bool testMode)
        {
            return Task.Factory.StartNew(() =>
            {
                var monitorItemOutput = new MonitorItemOutput();

                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return monitorItemOutput;
                }

                var systemValueTypes = _systemValueTypeService.GetAll();

                var svtServer = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_PingServer);
                var serverParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtServer.Id);

                Exception exception = null;
                PingReply pingReply = null;
                var actionItemParameters = new List<ActionItemParameter>();

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
                    pingReply = ping.Send(serverParam.Value, timeout, buffer, pingOptions);
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
                    foreach (var eventItem in eventItems)
                    {
                        if (IsEventValid(eventItem,  monitorItem, actionItemParameters, exception, pingReply))
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

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorPing, List<ActionItemParameter> actionItemParameters, Exception exception, PingReply pingReply)
        {
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                    case SystemValueTypes.ECS_PingReplyStatus:
                        meetsCondition = eventItem.EventCondition.IsValid(pingReply.Status);
                        break;
                }

            //if (meetsCondition)
            //{
            //    foreach(ActionItem actionItem in eventItem.ActionItems)
            //    {
            //        await ExecuteActionAsync(actionerList, monitorPing, actionItem, actionParameters);
            //    }
            //}
            
            return meetsCondition;
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.Ping;               
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
