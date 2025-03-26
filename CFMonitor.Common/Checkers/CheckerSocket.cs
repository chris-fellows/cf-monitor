using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFUtilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks socket (TCP/UDP)
    /// </summary>
    public class CheckerSocket : CheckerBase, IChecker
    {        
        public CheckerSocket(IAuditEventFactory auditEventFactory, 
            IAuditEventService auditEventService,
            IAuditEventTypeService auditEventTypeService, 
            IEventItemService eventItemService,
              IFileObjectService fileObjectService,
            IPlaceholderService placeholderService,
                        ISystemValueTypeService systemValueTypeService) : base(auditEventFactory, auditEventService, auditEventTypeService, eventItemService, fileObjectService, placeholderService, systemValueTypeService)
        {
            
        }

        public string Name => "TCP or UDP socket";

        //public CheckerTypes CheckerType => CheckerTypes.Socket;

        public Task<MonitorItemOutput> CheckAsync(MonitorAgent monitorAgent, MonitorItem monitorItem, CheckerConfig checkerConfig)
        {
            return Task.Factory.StartNew(() =>
            {
                SetPlaceholders(monitorAgent, monitorItem, checkerConfig);

                var monitorItemOutput = new MonitorItemOutput();

                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return monitorItemOutput;
                }

                var systemValueTypes = _systemValueTypeService.GetAll();

                var svtProtocol = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_SocketProtocol);
                var protocolParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtProtocol.Id);
                var protocol = GetValueWithPlaceholdersReplaced(protocolParam);

                var svtPort = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_SocketPort);
                var portParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtPort.Id);
                var port = GetValueWithPlaceholdersReplaced(portParam); 

                var svtHost = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_SocketHost);
                var hostParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtHost.Id);
                var host = GetValueWithPlaceholdersReplaced(hostParam);

                Exception exception = null;
                bool connected = false;
                var actionItemParameters = new List<ActionItemParameter>();

                try
                {
                    //switch (protocolParam.Value)
                    //{
                    //    case "TCP":
                    //        TcpSocket tcpSocket = new TcpSocket();
                    //        tcpSocket.Connect(host, Convert.ToInt32(port));
                    //        connected = tcpSocket.IsConnected;
                    //        tcpSocket.Disconnect();
                    //        break;
                    //    case "UDP":
                    //        UdpSocket udpSocket = new UdpSocket();                      
                    //        break;
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
                        if (IsEventValid(eventItem, monitorItem, actionItemParameters, exception, connected))
                        {
                            monitorItemOutput.EventItemIdsForAction.Add(eventItem.Id);
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                return monitorItemOutput;
            });
        }

        private bool IsEventValid(EventItem eventItem, MonitorItem monitorSocket, List<ActionItemParameter> actionItemParameters, Exception exception, bool connected)
        {            
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;
                    case SystemValueTypes.ECS_SocketConnected:
                        meetsCondition = eventItem.EventCondition.IsValid(connected);
                        break;
                }

            //if (meetsCondition)
            //{
            //    foreach (ActionItem actionItem in eventItem.ActionItems)
            //    {
            //        await ExecuteActionAsync(actionerList, monitorSocket, actionItem, actionParameters);
            //    }
            //}
            //

            return meetsCondition;
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.Socket;
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
