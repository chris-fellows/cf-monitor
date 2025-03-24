using CFConnectionMessaging.Interfaces;
using CFConnectionMessaging.Models;
using CFConnectionMessaging;
using CFMonitor.Constants;
using CFMonitor.Exceptions;
using CFMonitor.Models.Messages;
using CFMonitor.MessageConverters;
using CFMonitor.Common.MessageConverters;
using System.Diagnostics;

namespace CFMonitor.Agent
{
    /// <summary>
    /// Connection to Agent Manager (TCP)
    /// </summary>
    internal class ManagerConnection
    {        
        private ConnectionTcp _connection;

        // Message converters
        private IExternalMessageConverter<GetFileObjectRequest> _getFileObjectRequestConverter = new GetFileObjectRequestConverter();
        private IExternalMessageConverter<GetFileObjectResponse> _getFileObjectResponseConverter = new GetFileObjectResponseConverter();
        private IExternalMessageConverter<GetMonitorItemsRequest> _getMonitorItemsRequestConverter = new GetMonitorItemsRequestConverter();
        private IExternalMessageConverter<GetMonitorItemsResponse> _getMonitorItemsResponseConverter = new GetMonitorItemsResponseConverter();
        private IExternalMessageConverter<Heartbeat> _heartbeatConverter = new HeartbeatConverter();
        private IExternalMessageConverter<MonitorItemResultMessage> _monitorItemResultMessageConverter = new MonitorItemResultMessageConverter();
        private IExternalMessageConverter<MonitorItemUpdated> _monitorItemUpdatedConverter = new MonitorItemUpdatedConverter();

        //public delegate void GetMonitorItemsResponseReceived(GetMonitorItemsResponse response);
        //public event GetMonitorItemsResponseReceived? OnGetMonitorItemsResponseReceived;

        public delegate void MonitorItemUpdatedReceived(MonitorItemUpdated monitorItemUpdated);
        public event MonitorItemUpdatedReceived? OnMonitorItemUpdated;
            

        private List<MessageBase> _responseMessages = new List<MessageBase>();


        private TimeSpan _responseTimeout = TimeSpan.FromSeconds(30);

        public string LocalUserName { get; set; } = String.Empty;

        public ManagerConnection()
        {
            _connection = new ConnectionTcp();
            _connection.OnConnectionMessageReceived += _connection_OnConnectionMessageReceived;
        }

        public void StartListening(int port)
        {
            _connection.ReceivePort = port;
            _connection.StartListening();
        }

        public void StopListening()
        {
            _connection.StopListening();
        }

        /// <summary>
        /// Sends request to get monitor items
        /// </summary>
        /// <param name="chatMessage"></param>
        public GetMonitorItemsResponse SendGetMonitorItems(GetMonitorItemsRequest getMonitorItemsRequest, EndpointInfo remoteEndpointInfo)
        {
            // Send request
            _connection.SendMessage(_getMonitorItemsRequestConverter.GetConnectionMessage(getMonitorItemsRequest), remoteEndpointInfo);

            // Wait for response
            var responseMessages = new List<MessageBase>();
            var isGotAllMessages = WaitForResponses(getMonitorItemsRequest, _responseTimeout, _responseMessages,
                  (responseMessage) =>
                  {
                      responseMessages.Add(responseMessage);
                  });


            if (isGotAllMessages)
            {
                return (GetMonitorItemsResponse)responseMessages.First();
            }

            throw new MessageConnectionException("No response to get monitor items");
        }

        /// <summary>
        /// Send heartbeat
        /// </summary>
        /// <param name="heartbeat"></param>
        /// <param name="remoteEndpointInfo"></param>
        public void SendHeartbeat(Heartbeat heartbeat, EndpointInfo remoteEndpointInfo)
        {
            _connection.SendMessage(_heartbeatConverter.GetConnectionMessage(heartbeat), remoteEndpointInfo);
        }

        /// <summary>
        /// Sends monitor item result
        /// </summary>
        /// <param name="monitorItemResult"></param>
        /// <param name="remoteEndpointInfo"></param>
        public void SendMonitorItemResultMessage(MonitorItemResultMessage monitorItemResult, EndpointInfo remoteEndpointInfo)
        {
            _connection.SendMessage(_monitorItemResultMessageConverter.GetConnectionMessage(monitorItemResult), remoteEndpointInfo);
        }

        /// <summary>
        /// Handles ConnectionMessage received
        /// </summary>
        /// <param name="connectionMessage"></param>
        /// <param name="messageReceivedInfo"></param>
        private void _connection_OnConnectionMessageReceived(ConnectionMessage connectionMessage, MessageReceivedInfo messageReceivedInfo)
        {
            switch (connectionMessage.TypeId)
            {
                //case MessageTypeIds.GetMonitorItemsResponse:
                //    {
                //        // Get response
                //        var getMonitorItemsResponse = _getMonitorItemsResponseConverter.GetExternalMessage(connectionMessage);

                //        // Notify response
                //        if (OnGetMonitorItemsResponseReceived != null)
                //        {
                //            OnGetMonitorItemsResponseReceived(getMonitorItemsResponse);
                //        }
                //    }
                //    break;

                case MessageTypeIds.MonitorItemUpdated:
                    {
                        var monitorItemUpdated = _monitorItemUpdatedConverter.GetExternalMessage(connectionMessage);

                        if (OnMonitorItemUpdated != null)
                        {
                            OnMonitorItemUpdated(monitorItemUpdated);
                        }                        
                    }
                    break;
            }
        }

        /// <summary>
        /// Waits for all responses for request until completed or timeout. Where multiple responses are required then
        /// MessageBase.Response.IsMore=true for all except the last one.
        /// </summary>
        /// <param name="request">Request to check</param>
        /// <param name="timeout">Timeout receiving responses</param>
        /// <param name="responseMessagesToCheck">List where responses are added</param>
        /// <param name="responseMessageAction">Action to forward next response</param>
        /// <returns>Whether all responses received</returns>
        private bool WaitForResponses(MessageBase request, TimeSpan timeout,
                                      List<MessageBase> responseMessagesToCheck,
                                      Action<MessageBase> responseMessageAction)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var isGotAllResponses = false;
            while (!isGotAllResponses &&
                    stopwatch.Elapsed < timeout)
            {
                // Check for next response message
                var responseMessage = responseMessagesToCheck.FirstOrDefault(m => m.Response != null && m.Response.MessageId == request.Id);

                if (responseMessage != null)
                {
                    // Discard
                    responseMessagesToCheck.Remove(responseMessage);

                    // Check if last response
                    isGotAllResponses = !responseMessage.Response.IsMore;

                    // Pass response to caller
                    responseMessageAction(responseMessage);
                }

                Thread.Sleep(20);
            }

            return isGotAllResponses;
        }
    }
}
