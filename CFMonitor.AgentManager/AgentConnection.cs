using CFConnectionMessaging.Interfaces;
using CFConnectionMessaging.Models;
using CFConnectionMessaging;
using CFMonitor.Common.MessageConverters;
using CFMonitor.Constants;
using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.MessageConverters;
using CFMonitor.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Markup;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

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
        private IExternalMessageConverter<MonitorItemResultMessage> _monitorItemResultMessageConverter = new MonitorItemResultMessageConverter();
        private IExternalMessageConverter<MonitorItemUpdated> _monitorItemUpdatedConverter = new MonitorItemUpdatedConverter();
        
        //public delegate void GetMonitorItemsRequestReceived(GetMonitorItemsRequest request);
        //public event GetMonitorItemsRequestReceived? OnGetMonitorItemsRequestReceived;

        //public delegate void HeartbeatReceived(Heartbeat heartbeat);
        //public event HeartbeatReceived? OnHeartbeatReceived;

        private List<MessageBase> _responseMessages = new List<MessageBase>();

        public string LocalUserName { get; set; } = String.Empty;

        private readonly IAuditEventFactory _auditEventFactory;
        private readonly IAuditEventService _auditEventService;
        private readonly IEventItemService _eventItemService;        
        private readonly IMonitorAgentService _monitorAgentService;
        private readonly IMonitorItemOutputService _monitorItemOutputService;
        private readonly IMonitorItemService _monitorItemService;
        private readonly IServiceProvider _serviceProvider;

        public AgentConnection(IAuditEventFactory auditEventFactory,
                                IAuditEventService auditEventService,
                                IEventItemService eventItemService,
                                IMonitorAgentService monitorAgentService,
                               IMonitorItemOutputService monitorItemOutputService,
                               IMonitorItemService monitorItemService,
                               IServiceProvider serviceProvider)
            
        {
            _auditEventFactory = auditEventFactory;
            _auditEventService = auditEventService;
            _eventItemService = eventItemService;
            _monitorAgentService = monitorAgentService;
            _monitorItemOutputService = monitorItemOutputService;
            _monitorItemService = monitorItemService;
            _serviceProvider = serviceProvider;

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

                case MessageTypeIds.MonitorItemResultMessage:
                    var monitorItemResultMessage = _monitorItemResultMessageConverter.GetExternalMessage(connectionMessage);

                    ProcessMonitorItemResultAsync(monitorItemResultMessage, messageReceivedInfo);
                    break;
            }
        }

        private Task ProcessGetMonitorItemsRequestAsync(GetMonitorItemsRequest getMonitorItemsRequest, MessageReceivedInfo messageReceivedInfo)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var monitorItemService = scope.ServiceProvider.GetRequiredService<IMonitorItemService>();

                    var monitorItems = monitorItemService.GetAll();

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
                }
            });
        }

        /// <summary>
        /// Processes Monitor Agent heartbeat. If first heartbeat then creates MonitorAgent in DB.
        /// </summary>
        /// <param name="heartbeat"></param>
        /// <param name="messageReceivedInfo"></param>
        /// <returns></returns>
        private Task ProcessHeartbeatAsync(Heartbeat heartbeat, MessageReceivedInfo messageReceivedInfo)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var monitorAgentGroupService = scope.ServiceProvider.GetRequiredService<IMonitorAgentGroupService>();
                    var monitorAgentService = scope.ServiceProvider.GetRequiredService<IMonitorAgentService>();

                    // Get monitor agent                
                    var monitorAgent = monitorAgentService.GetById(heartbeat.SenderAgentId);
                    if (monitorAgent != null)       // Known monitor agent
                    {
                        monitorAgent.HeartbeatDateTime = DateTimeOffset.UtcNow;
                        monitorAgent.MachineName = heartbeat.MachineName;
                        monitorAgent.UserName = heartbeat.UserName;
                        monitorAgent.IP = messageReceivedInfo.RemoteEndpointInfo.Ip;
                        monitorAgent.Port = messageReceivedInfo.RemoteEndpointInfo.Port;

                        monitorAgentService.Update(monitorAgent);
                    }
                    else if (heartbeat.SenderAgentId.Length != Guid.Empty.ToString().Length)    // New agent (Ignore if Id is unexpected format)
                    {
                        // Default to first group, user can change later
                        var monitorAgentGroup = monitorAgentGroupService.GetAll().OrderBy(g => g.Name).First();

                        monitorAgent = new MonitorAgent()
                        {
                            Id = heartbeat.SenderAgentId,
                            MonitorAgentGroupId = monitorAgentGroup.Id,
                            HeartbeatDateTime = DateTimeOffset.UtcNow,
                            MachineName = heartbeat.MachineName,
                            UserName = heartbeat.UserName,
                            IP = messageReceivedInfo.RemoteEndpointInfo.Ip,
                            Port = messageReceivedInfo.RemoteEndpointInfo.Port,
                        };

                        monitorAgentService.Add(monitorAgent);
                    }
                }
            });
        }

        /// <summary>
        /// Processes monitor item result. Saves MonitorItemOutput and then executes action(s)
        /// </summary>
        /// <param name="monitorItemResultMessage"></param>
        /// <param name="messageReceivedInfo"></param>
        /// <returns></returns>
        private Task ProcessMonitorItemResultAsync(MonitorItemResultMessage monitorItemResultMessage, MessageReceivedInfo messageReceivedInfo)
        {
            return Task.Factory.StartNew(() =>
            {                                
                if (monitorItemResultMessage.MonitorItemOutput != null)
                {
                    // Save monitor item output
                    var monitorItemOutput = monitorItemResultMessage.MonitorItemOutput;
                    _monitorItemOutputService.Add(monitorItemOutput);

                    // Add "Checked monitor item" audit event
                    _auditEventService.Add(_auditEventFactory.CreateCheckedMonitorItem(monitorItemOutput.Id));

                    // Execute action(s)
                    if (monitorItemOutput.EventItemIdsForAction != null &&
                        monitorItemOutput.EventItemIdsForAction.Any())
                    {                        
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            // Get services
                            var auditEventFactory = scope.ServiceProvider.GetRequiredService<IAuditEventFactory>();
                            var auditEventService = scope.ServiceProvider.GetRequiredService<IAuditEventService>();
                            var monitorItemService = scope.ServiceProvider.GetRequiredService<IMonitorItemService>();
                            var systemValueTypeService = scope.ServiceProvider.GetRequiredService<ISystemValueTypeService>();

                            // Get monitor item
                            var monitorItem = monitorItemService.GetById(monitorItemOutput.MonitorItemId);

                            foreach (var eventItemId in monitorItemOutput.EventItemIdsForAction)
                            {
                                // Get event item
                                var eventItem = _eventItemService.GetById(eventItemId);

                                // Execute actions
                                // TODO: Limit concurrent actions
                                if (eventItem != null && eventItem.ActionItems.Any())
                                {
                                    var actionTasksNByActionItemId = new Dictionary<string, Task>();
                                    foreach (var actionItem in eventItem.ActionItems)
                                    {
                                        // Create action scope to ensure that we don't use shared resources unless intended
                                        using (var actionScope = _serviceProvider.CreateScope())
                                        {
                                            // Get actioner
                                            var actioner = actionScope.ServiceProvider.GetServices<IActioner>().First(a => a.CanExecute(actionItem));

                                            // Start action execute
                                            actionTasksNByActionItemId.Add(actionItem.Id, actioner.ExecuteAsync(monitorItem, actionItem, monitorItemOutput.ActionItemParameters));
                                        }
                                    }

                                    // Wait for actions to complete
                                    // TODO: Check errors
                                    Task.WhenAll(actionTasksNByActionItemId.Select(k => k.Value).ToArray());

                                    // Add "Action executed" (or "Error") audit event
                                    var systemValueTypes = systemValueTypeService.GetAll();
                                    foreach (var actionItemId in actionTasksNByActionItemId.Keys)
                                    {
                                        if (actionTasksNByActionItemId[actionItemId].Exception == null)    // Action executed
                                        {
                                            auditEventService.Add(auditEventFactory.CreateActionExecuted(monitorItemOutput.Id, actionItemId));                                            
                                        }
                                        else    // Add error audit event
                                        {
                                            // Note: MonitorItemOutput indicates MonitorItemId & MonitorAgentId
                                            auditEventService.Add(auditEventFactory.CreateError($"Error executing action item: {actionTasksNByActionItemId[actionItemId].Exception.Message}",
                                                            new List<AuditEventParameter>()
                                                            {
                                                                //new AuditEventParameter()
                                                                //{
                                                                //    SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_MonitorItemId).Id,
                                                                //    Value = monitorItem.Id,
                                                                //},
                                                                 new AuditEventParameter()
                                                                {
                                                                    SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_MonitorItemOutputId).Id,
                                                                    Value = monitorItemOutput.Id,
                                                                },
                                                                new AuditEventParameter()
                                                                {
                                                                    SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_ActionItemId).Id,
                                                                    Value = actionItemId,
                                                                },
                                                                //   new AuditEventParameter()
                                                                //{
                                                                //    SystemValueTypeId = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AEP_MonitorAgentId).Id,
                                                                //    Value = monitorItemOutput.MonitorAgentId,
                                                                //}
                                                            }));                                                    
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
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
