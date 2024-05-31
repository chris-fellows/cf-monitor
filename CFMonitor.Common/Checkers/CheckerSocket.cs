using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using CFUtilities.Networking.Socket;
using System;
using System.Collections.Generic;

namespace CFMonitor
{
    /// <summary>
    /// Checks socket (TCP/UDP)
    /// </summary>
    public class CheckerSocket : IChecker
    {
        public void Check(MonitorItem monitorItem, List<IActioner> actionerList)
        {
            MonitorSocket monitorSocket = (MonitorSocket)monitorItem;
            Exception exception = null;
            bool connected = false;
            ActionParameters actionParameters = new ActionParameters();

            try
            {
                switch (monitorSocket.Protocol)
                {
                    case "TCP":
                        TcpSocket tcpSocket = new TcpSocket();
                        tcpSocket.Connect(monitorSocket.Host, monitorSocket.Port);
                        connected = tcpSocket.IsConnected;
                        tcpSocket.Disconnect();
                        break;
                    case "UDP":
                        UdpSocket udpSocket = new UdpSocket();                      
                        break;
                }

            }
            catch (Exception ex)
            {
                exception = ex;
            }

            try
            {
                CheckEvents(actionerList, monitorSocket, actionParameters, exception, connected);
            }
            catch (Exception ex)
            {
                
            }
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorSocket monitorSocket, ActionParameters actionParameters, Exception exception, bool connected)
        {
            foreach (EventItem eventItem in monitorSocket.EventItems)
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
                    case EventConditionSource.SocketConnected:
                        meetsCondition = connected;
                        break;
                    case EventConditionSource.SocketNotConnected:
                        meetsCondition = !connected;
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
                else if (eventItem.EventCondition.Source.Equals("OnConnected"))
                {
                    meetsCondition = connected;
                }
                else if (eventItem.EventCondition.Source.Equals("OnNotConnected"))
                {
                    meetsCondition = !connected;
                }
                */

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        DoAction(actionerList, monitorSocket, actionItem, actionParameters);
                    }
                }
            }
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem is MonitorSocket;
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
