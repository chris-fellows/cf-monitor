﻿using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
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
        public CheckerSocket(IEventItemService eventItemService,
                        ISystemValueTypeService systemValueTypeService) : base(eventItemService, systemValueTypeService)
        {
            
        }

        public string Name => "TCP or UDP socket";

        //public CheckerTypes CheckerType => CheckerTypes.Socket;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            return Task.Factory.StartNew(async () =>
            {
                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return;
                }

                var systemValueTypes = _systemValueTypeService.GetAll();

                var svtProtocol = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_SocketProtocol);
                var protocolParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtProtocol.Id);

                var svtPort = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_SocketPort);
                var portParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtPort.Id);

                var svtHost = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_SocketHost);
                var hostParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtHost.Id);

                Exception exception = null;
                bool connected = false;
                ActionParameters actionParameters = new ActionParameters();

                try
                {
                    //switch (protocolParam.Value)
                    //{
                    //    case "TCP":
                    //        TcpSocket tcpSocket = new TcpSocket();
                    //        tcpSocket.Connect(hostParam.Value, Convert.ToInt32(portParam.Value));
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
                        await CheckEventAsync(eventItem, actionerList, monitorItem, actionParameters, exception, connected);
                    }
                }
                catch (Exception ex)
                {

                }
            });
        }

        private async Task CheckEventAsync(EventItem eventItem, List<IActioner> actionerList, MonitorItem monitorSocket, ActionParameters actionParameters, Exception exception, bool connected)
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

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        await ExecuteActionAsync(actionerList, monitorSocket, actionItem, actionParameters);
                    }
                }            
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
