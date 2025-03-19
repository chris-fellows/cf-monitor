using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using CFUtilities.Networking.Socket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks socket (TCP/UDP)
    /// </summary>
    public class CheckerSocket : IChecker
    {
        public string Name => "TCP or UDP socket";

        public CheckerTypes CheckerType => CheckerTypes.Socket;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            //MonitorSocket monitorSocket = (MonitorSocket)monitorItem;
            var protocolParam = monitorItem.Parameters.First(p => p.SystemValueType == SystemValueTypes.MIP_SocketProtocol);
            var portParam = monitorItem.Parameters.First(p => p.SystemValueType == SystemValueTypes.MIP_SocketPort);
            var hostParam = monitorItem.Parameters.First(p => p.SystemValueType == SystemValueTypes.MIP_SocketHost);

            Exception exception = null;
            bool connected = false;
            ActionParameters actionParameters = new ActionParameters();

            try
            {
                switch (protocolParam.Value)
                {
                    case "TCP":
                        TcpSocket tcpSocket = new TcpSocket();
                        tcpSocket.Connect(hostParam.Value, Convert.ToInt32(portParam.Value));
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
                CheckEvents(actionerList, monitorItem, actionParameters, exception, connected);
            }
            catch (Exception ex)
            {
                
            }

            return Task.CompletedTask;
        }

        private void CheckEvents(List<IActioner> actionerList, MonitorItem monitorSocket, ActionParameters actionParameters, Exception exception, bool connected)
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
            return monitorItem.MonitorItemType == MonitorItemTypes.Socket;
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
