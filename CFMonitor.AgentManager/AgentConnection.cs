using CFConnectionMessaging.Interfaces;
using CFConnectionMessaging.Models;
using CFConnectionMessaging;
using CFMonitor.Common.MessageConverters;
using CFMonitor.Constants;
using CFMonitor.MessageConverters;
using CFMonitor.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Markup;
using CFMonitor.Interfaces;

namespace CFMonitor.AgentManager
{
    /// <summary>
    /// Connection to Agent instances
    /// </summary>
    internal class AgentConnection
    {
        private ConnectionTcp _connection;

        // Message converters
        private IExternalMessageConverter<GetFileObjectRequest> _getFileObjectRequestConverter = new GetFileObjectRequestConverter();
        private IExternalMessageConverter<GetFileObjectResponse> _getFileObjectResponseConverter = new GetFileObjectResponseConverter();
        private IExternalMessageConverter<GetMonitorItemsRequest> _getMonitorItemsRequestConverter = new GetMonitorItemsRequestConverter();
        private IExternalMessageConverter<GetMonitorItemsResponse> _getMonitorItemsResponseConverter = new GetMonitorItemsResponseConverter();
        private IExternalMessageConverter<Heartbeat> _heartbeatConverter = new HeartbeatConverter();
        private IExternalMessageConverter<MonitorItemResult> _monitorItemResultConverter = new MonitorItemResultConverter();
        private IExternalMessageConverter<MonitorItemUpdated> _monitorItemUpdatedConverter = new MonitorItemUpdatedConverter();
        
        //public delegate void GetMonitorItemsRequestReceived(GetMonitorItemsRequest request);
        //public event GetMonitorItemsRequestReceived? OnGetMonitorItemsRequestReceived;

        //public delegate void HeartbeatReceived(Heartbeat heartbeat);
        //public event HeartbeatReceived? OnHeartbeatReceived;

        private List<MessageBase> _responseMessages = new List<MessageBase>();

        public string LocalUserName { get; set; } = String.Empty;

        private readonly IMonitorAgentService _monitorAgentService;
        private readonly IMonitorItemService _monitorItemService;

        public AgentConnection(IMonitorAgentService monitorAgentService,
                    IMonitorItemService monitorItemService)
            
        {
            _monitorAgentService = monitorAgentService;
            _monitorItemService = monitorItemService;

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

        ///// <summary>
        ///// Sends request to get monitor items
        ///// </summary>
        ///// <param name="chatMessage"></param>
        //public void SendGetMonitorItems(GetMonitorItemsRequest getMonitorItemsRequest, EndpointInfo remoteEndpointInfo)
        //{
        //    _connection.SendMessage(_getMonitorItemsRequestConverter.GetConnectionMessage(getMonitorItemsRequest), remoteEndpointInfo);
        //}

        /// <summary>
        /// Send monitor item updated notification
        /// </summary>
        /// <param name="heartbeat"></param>
        public void SendMonitorItemUpdated(MonitorItemUpdated monitorItemUpdated, EndpointInfo remoteEndpointInfo)
        {
            _connection.SendMessage(_monitorItemUpdatedConverter.GetConnectionMessage(monitorItemUpdated), remoteEndpointInfo);
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
                case MessageTypeIds.GetMonitorItemsRequest:
                    var getMonitorItemsRequest = _getMonitorItemsRequestConverter.GetExternalMessage(connectionMessage);

                    ProcessGetMonitorItemsRequestAsync(getMonitorItemsRequest, messageReceivedInfo);
                    break;

                case MessageTypeIds.Heartbeat:
                    var heartbeat = _heartbeatConverter.GetExternalMessage(connectionMessage);

                    ProcessHeartbeatAsync(heartbeat, messageReceivedInfo);          
                    break;

                case MessageTypeIds.MonitorItemResult:
                    var monitorItemResult = _monitorItemResultConverter.GetExternalMessage(connectionMessage);

                    ProcessMonitorItemResultAsync(monitorItemResult, messageReceivedInfo);
                    break;
            }
        }

        private Task ProcessGetMonitorItemsRequestAsync(GetMonitorItemsRequest getMonitorItemsRequest, MessageReceivedInfo messageReceivedInfo)
        {
            return Task.Factory.StartNew(() =>
            {
                var monitorItems = _monitorItemService.GetAll();

                var getMonitorItemsResponse = new GetMonitorItemsResponse()
                {
                    Id = Guid.NewGuid().ToString(),
                    MonitorItems = monitorItems,
                    Response = new MessageResponse()
                    {
                        IsMore = false,
                        MessageId = getMonitorItemsRequest.Id,
                        Sequence = 1
                    }
                };

                // Send response
                _connection.SendMessage(_getMonitorItemsResponseConverter.GetConnectionMessage(getMonitorItemsResponse), messageReceivedInfo.RemoteEndpointInfo);
            });
        }

        private Task ProcessHeartbeatAsync(Heartbeat heartbeat, MessageReceivedInfo messageReceivedInfo)
        {
            return Task.Factory.StartNew(() =>
            {
                // Get monitor agent
                var monitorAgent = _monitorAgentService.GetById(heartbeat.SenderAgentId);
                if (monitorAgent != null)       // Known monitor agent
                {
                    monitorAgent.HeartbeatDateTime = DateTimeOffset.UtcNow;
                    _monitorAgentService.Update(monitorAgent);
                }
            });
        }

        private Task ProcessMonitorItemResultAsync(MonitorItemResult monitorItemResult, MessageReceivedInfo messageReceivedInfo)
        {
            // TODO: Implement
            // We need to decide how we execute the action(s).
            // Possibly we need a queue.
            return Task.Factory.StartNew(() =>
            {
            });
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
