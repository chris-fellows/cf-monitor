﻿using CFConnectionMessaging.Interfaces;
using CFConnectionMessaging.Models;
using CFConnectionMessaging;
using CFMonitor.Constants;
using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.MessageConverters;
using CFMonitor.Models.Messages;
using CFMonitor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Markup;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using CFMonitor.Log;
using System.Collections.Concurrent;
using CFMonitor.AgentManager.Models;

namespace CFMonitor.AgentManager
{
    /// <summary>
    /// Connection to Agent instances. Received messages are passed via OnConnectionMessageReceived and then processed.
    /// </summary>
    internal class AgentsConnection
    {
        private ConnectionTcp _connection;

        private readonly MessageConvertersList _messageConverters = new();
        
        //public delegate void GetMonitorItemsRequestReceived(GetMonitorItemsRequest request);
        //public event GetMonitorItemsRequestReceived? OnGetMonitorItemsRequestReceived;

        //public delegate void HeartbeatReceived(Heartbeat heartbeat);
        //public event HeartbeatReceived? OnHeartbeatReceived;

        //private List<MessageBase> _responseMessages = new List<MessageBase>();

        private readonly IAuditEventFactory _auditEventFactory;
        private readonly IAuditEventService _auditEventService;
        private readonly IEventItemService _eventItemService;
        private readonly IFileObjectService _fileObjectService;
        private readonly IMonitorAgentService _monitorAgentService;
        private readonly IMonitorItemOutputService _monitorItemOutputService;
        private readonly IMonitorItemService _monitorItemService;        
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserService _userService;

        private DateTimeOffset _monitorAgentsLastRefreshTime = DateTimeOffset.MinValue;
        private ConcurrentDictionary<string, MonitorAgent> _monitorAgentsBySecurityKey = new();

        public delegate void ConnectionMessageReceived(ConnectionMessage connectionMessage, MessageReceivedInfo messageReceivedInfo);
        public event ConnectionMessageReceived? OnConnectionMessageReceived;

        private readonly ISimpleLog _log;             

        public AgentsConnection(IAuditEventFactory auditEventFactory,
                                IAuditEventService auditEventService,
                                IEventItemService eventItemService,
                                IFileObjectService fileObjectService,
                                ISimpleLog log,
                                IMonitorAgentService monitorAgentService,
                               IMonitorItemOutputService monitorItemOutputService,
                               IMonitorItemService monitorItemService,
                               IServiceProvider serviceProvider,
                               IUserService userService)
            
        {
            
            _auditEventFactory = auditEventFactory;
            _auditEventService = auditEventService;
            _eventItemService = eventItemService;
            _fileObjectService = fileObjectService;
            _log = log;
            _monitorAgentService = monitorAgentService;
            _monitorItemOutputService = monitorItemOutputService;
            _monitorItemService = monitorItemService;
            _serviceProvider = serviceProvider;
            _userService = userService;

            _connection = new ConnectionTcp();           
            _connection.OnConnectionMessageReceived += delegate(ConnectionMessage connectionMessage, MessageReceivedInfo messageReceivedInfo)
            {                
                if (IsResponseMessage(connectionMessage))     // Inside Send... method waiting for response
                {
                    // No current requests do this
                }
                else
                {
                    if (OnConnectionMessageReceived != null)
                    {
                        OnConnectionMessageReceived(connectionMessage, messageReceivedInfo);
                    }
                }
            };
        }

        public void StartListening(int port)
        {
            _log.Log(DateTimeOffset.UtcNow, "Information", $"Listening on port {port}");

            _connection.ReceivePort = port;
            _connection.StartListening();            
        }

        public void StopListening()
        {
            _log.Log(DateTimeOffset.UtcNow, "Information", "Stopping listening");
            _connection.StopListening();
        }     

        /// <summary>
        /// Send entity updated notification. E.g. MonitorItem updated
        /// </summary>
        /// <param name="heartbeat"></param>
        public void SendEntityUpdated(EntityUpdated monitorItemUpdated, EndpointInfo remoteEndpointInfo)
        {
            _connection.SendMessage(_messageConverters.EntityUpdatedConverter.GetConnectionMessage(monitorItemUpdated), remoteEndpointInfo);
        }

        private bool IsResponseMessage(ConnectionMessage connectionMessage)
        {
            var responseMessageTypeIds = new[]
            {
                 MessageTypeIds.GetEventItemsResponse,
                 MessageTypeIds.GetFileObjectResponse,
                 MessageTypeIds.GetMonitorAgentsResponse,
                 MessageTypeIds.GetMonitorItemsResponse,
                 MessageTypeIds.GetSystemValueTypesResponse
            };

            return responseMessageTypeIds.Contains(connectionMessage.TypeId);
        }

        ///// <summary>
        ///// Handles ConnectionMessage received
        ///// </summary>
        ///// <param name="connectionMessage"></param>
        ///// <param name="messageReceivedInfo"></param>
        //private void _connection_OnConnectionMessageReceived(ConnectionMessage connectionMessage, MessageReceivedInfo messageReceivedInfo)
        //{
        //    var queueItem = new QueueItem()
        //    {
        //        ConnectionMessage = connectionMessage,
        //        MessageReceivedInfo = messageReceivedInfo
        //    };
        //    _queue.Enqueue(queueItem);           
        //}

        /// <summary>
        /// Handles connection message
        /// </summary>
        /// <param name="connectionMessage"></param>
        /// <param name="messageReceivedInfo"></param>
        /// <returns></returns>
        public Task HandleConnectionMessageAsync(ConnectionMessage connectionMessage, MessageReceivedInfo messageReceivedInfo)
        {
            switch (connectionMessage.TypeId)
            {
                case MessageTypeIds.GetEventItemsRequest:
                    var getEventItemsRequest = _messageConverters.GetEventItemsRequestConverter.GetExternalMessage(connectionMessage);

                    return HandleGetEventItemsRequestAsync(getEventItemsRequest, messageReceivedInfo);                    

                case MessageTypeIds.GetFileObjectRequest:
                    var getFileObjectRequest = _messageConverters.GetFileObjectRequestConverter.GetExternalMessage(connectionMessage);

                    return HandleGetFileObjectRequestAsync(getFileObjectRequest, messageReceivedInfo);                    

                case MessageTypeIds.GetMonitorAgentsRequest:
                    var getMonitorAgentsRequest = _messageConverters.GetMonitorAgentsRequestConverter.GetExternalMessage(connectionMessage);

                    return HandleGetMonitorAgentsRequestAsync(getMonitorAgentsRequest, messageReceivedInfo);                    

                case MessageTypeIds.GetMonitorItemsRequest:
                    var getMonitorItemsRequest = _messageConverters.GetMonitorItemsRequestConverter.GetExternalMessage(connectionMessage);

                    return HandleGetMonitorItemsRequestAsync(getMonitorItemsRequest, messageReceivedInfo);                    

                case MessageTypeIds.GetSystemValueTypesRequest:
                    var getSystemValueTypesRequest = _messageConverters.GetSystemValueTypesRequestConverter.GetExternalMessage(connectionMessage);

                    return HandleGetSystemValueTypesRequestAsync(getSystemValueTypesRequest, messageReceivedInfo);                    

                case MessageTypeIds.Heartbeat:
                    var heartbeat = _messageConverters.HeartbeatConverter.GetExternalMessage(connectionMessage);

                    return HandleHeartbeatAsync(heartbeat, messageReceivedInfo);                     

                case MessageTypeIds.MonitorAgentLogMessage:
                    var monitorAgentLogMessage = _messageConverters.MonitorAgentLogMessageConverer.GetExternalMessage(connectionMessage);

                    return HandleMonitorAgentLogMessageAsync(monitorAgentLogMessage, messageReceivedInfo);                    

                case MessageTypeIds.MonitorItemResultMessage:
                    var monitorItemResultMessage = _messageConverters.MonitorItemResultMessageConverter.GetExternalMessage(connectionMessage);

                    return HandleMonitorItemResultAsync(monitorItemResultMessage, messageReceivedInfo);                    
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets monitor agent by security key
        /// </summary>
        /// <param name="securityKey"></param>
        /// <returns></returns>
        private MonitorAgent? GetMonitorAgentBySecurityKey(string securityKey)
        {
            // Periodically clear cache. Doesn't matter too much if we pick up stale data as the user settings won't change
            // very often.
            if (_monitorAgentsLastRefreshTime.AddMinutes(5) <= DateTimeOffset.UtcNow)
            {
                _monitorAgentsLastRefreshTime = DateTimeOffset.UtcNow;
                _monitorAgentsBySecurityKey.Clear();
            }
            if (!_monitorAgentsBySecurityKey.Any())   // Cache empty, load it
            {
                var monitorAgents = _monitorAgentService.GetAll();
                foreach(var monitorAgent in monitorAgents)
                {
                    _monitorAgentsBySecurityKey.TryAdd(monitorAgent.SecurityKey, monitorAgent);
                }
                //_monitorAgentService.GetAll().ForEach(monitorAgent => _monitorAgentsBySecurityKey.Add(monitorAgent.SecurityKey, monitorAgent));
            }
            return _monitorAgentsBySecurityKey.ContainsKey(securityKey) ? _monitorAgentsBySecurityKey[securityKey] : null;
        }
        
        private Task HandleMonitorAgentLogMessageAsync(MonitorAgentLogMessage monitorAgentLogMessage, MessageReceivedInfo messageReceivedInfo)
        {
            return Task.Factory.StartNew(() =>
            {
                _log.Log(DateTimeOffset.UtcNow, "Information", $"Processing {monitorAgentLogMessage.TypeId} from Monitor Agent {monitorAgentLogMessage.SenderAgentId}");

                using (var scope = _serviceProvider.CreateScope())
                {
                    // Save log
                    var fileObject = new FileObject()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = $"{monitorAgentLogMessage.SenderAgentId}.{monitorAgentLogMessage.FileName}",
                        Content = monitorAgentLogMessage.Content
                    };
                    _fileObjectService.AddAsync(fileObject).Wait();

                    _log.Log(DateTimeOffset.UtcNow, "Information", $"Processed {monitorAgentLogMessage.TypeId} from Monitor Agent {monitorAgentLogMessage.SenderAgentId}");
                }
            });
        }

        private Task HandleGetFileObjectRequestAsync(GetFileObjectRequest getFileObjectRequest, MessageReceivedInfo messageReceivedInfo)
        {
            return Task.Factory.StartNew(() =>
            {
                _log.Log(DateTimeOffset.UtcNow, "Information", $"Processing {getFileObjectRequest.TypeId} from Monitor Agent {getFileObjectRequest.SenderAgentId}");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var getFileObjectResponse = new GetFileObjectResponse()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Response = new MessageResponse()
                        {
                            IsMore = false,
                            MessageId = getFileObjectRequest.Id,
                            Sequence = 1
                        }
                    };

                    var monitorAgent = GetMonitorAgentBySecurityKey(getFileObjectRequest.SecurityKey);

                    if (monitorAgent == null)
                    {
                        getFileObjectResponse.Response.ErrorCode = ResponseErrorCodes.PermissionDenied;
                        getFileObjectResponse.Response.ErrorMessage = EnumUtilities.GetEnumDescription(getFileObjectResponse.Response.ErrorCode);
                    }
                    else
                    {
                        var fileObject = _fileObjectService.GetByIdAsync(getFileObjectRequest.FileObjectId).Result;

                        getFileObjectResponse.FileObject = fileObject;
                    }

                    // Send response
                    _connection.SendMessage(_messageConverters.GetFileObjectResponseConverter.GetConnectionMessage(getFileObjectResponse), messageReceivedInfo.RemoteEndpointInfo);

                    var error = getFileObjectResponse.Response.ErrorCode == null ? "Success" : getFileObjectResponse.Response.ErrorMessage;
                    _log.Log(DateTimeOffset.UtcNow, "Information", $"Processed {getFileObjectRequest.TypeId} from Monitor Agent {getFileObjectRequest.SenderAgentId}: {error}");
                }
            });
        }

        private Task HandleGetEventItemsRequestAsync(GetEventItemsRequest getEventItemsRequest, MessageReceivedInfo messageReceivedInfo)
        {
            return Task.Factory.StartNew(() =>
            {
                _log.Log(DateTimeOffset.UtcNow, "Information", $"Processing {getEventItemsRequest.TypeId} from Monitor Agent {getEventItemsRequest.SenderAgentId}");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var getEventItemsResponse = new GetEventItemsResponse()
                    {
                        Id = Guid.NewGuid().ToString(),                      
                        Response = new MessageResponse()
                        {
                            IsMore = false,
                            MessageId = getEventItemsRequest.Id,
                            Sequence = 1
                        }
                    };

                    var monitorAgent = GetMonitorAgentBySecurityKey(getEventItemsRequest.SecurityKey);

                    if (monitorAgent == null)
                    {
                        getEventItemsResponse.Response.ErrorCode = ResponseErrorCodes.PermissionDenied;
                        getEventItemsResponse.Response.ErrorMessage = EnumUtilities.GetEnumDescription(getEventItemsResponse.Response.ErrorCode);
                    }
                    else
                    {
                        var eventItemService = scope.ServiceProvider.GetRequiredService<IEventItemService>();

                        getEventItemsResponse.EventItems = eventItemService.GetAll();
                    }

                    // Send response
                    _connection.SendMessage(_messageConverters.GetEventItemsResponseConverter.GetConnectionMessage(getEventItemsResponse), messageReceivedInfo.RemoteEndpointInfo);

                    var error = getEventItemsResponse.Response.ErrorCode == null ? "Success" : getEventItemsResponse.Response.ErrorMessage;
                    _log.Log(DateTimeOffset.UtcNow, "Information", $"Processed {getEventItemsRequest.TypeId} from Monitor Agent {getEventItemsRequest.SenderAgentId}: {error}");
                }                                
            });
        }

        private Task HandleGetMonitorAgentsRequestAsync(GetMonitorAgentsRequest getMonitorAgentsRequest, MessageReceivedInfo messageReceivedInfo)
        {
            return Task.Factory.StartNew(() =>
            {
                _log.Log(DateTimeOffset.UtcNow, "Information", $"Processing {getMonitorAgentsRequest.TypeId} from Monitor Agent {getMonitorAgentsRequest.SenderAgentId}");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var getMonitorAgentsResponse = new GetMonitorAgentsResponse()
                    {
                        Id = Guid.NewGuid().ToString(),                     
                        Response = new MessageResponse()
                        {
                            IsMore = false,
                            MessageId = getMonitorAgentsRequest.Id,
                            Sequence = 1
                        }
                    };

                    var monitorAgent = GetMonitorAgentBySecurityKey(getMonitorAgentsRequest.SecurityKey);

                    if (monitorAgent == null)
                    {                        
                        getMonitorAgentsResponse.Response.ErrorCode = ResponseErrorCodes.PermissionDenied;
                        getMonitorAgentsResponse.Response.ErrorMessage = EnumUtilities.GetEnumDescription(getMonitorAgentsResponse.Response.ErrorCode);
                    }
                    else
                    {
                        var monitorAgentService = scope.ServiceProvider.GetRequiredService<IMonitorAgentService>();

                        getMonitorAgentsResponse.MonitorAgents = monitorAgentService.GetAll();
                    }

                    // Send response
                    _connection.SendMessage(_messageConverters.GetMonitorAgentsResponseConverter.GetConnectionMessage(getMonitorAgentsResponse), messageReceivedInfo.RemoteEndpointInfo);

                    var error = getMonitorAgentsResponse.Response.ErrorCode == null ? "Success" : getMonitorAgentsResponse.Response.ErrorMessage;
                    _log.Log(DateTimeOffset.UtcNow, "Information", $"Processed {getMonitorAgentsRequest.TypeId} from Monitor Agent {getMonitorAgentsRequest.SenderAgentId}: {error}");
                }                
            });
        }

        private Task HandleGetMonitorItemsRequestAsync(GetMonitorItemsRequest getMonitorItemsRequest, MessageReceivedInfo messageReceivedInfo)
        {
            return Task.Factory.StartNew(() =>
            {
                _log.Log(DateTimeOffset.UtcNow, "Information", $"Processing {getMonitorItemsRequest.TypeId} from Monitor Agent {getMonitorItemsRequest.SenderAgentId}");

                using (var scope = _serviceProvider.CreateScope())
                {                    
                    var getMonitorItemsResponse = new GetMonitorItemsResponse()
                    {
                        Id = Guid.NewGuid().ToString(),                        
                        Response = new MessageResponse()
                        {
                            IsMore = false,
                            MessageId = getMonitorItemsRequest.Id,
                            Sequence = 1
                        }
                    };

                    var monitorAgent = GetMonitorAgentBySecurityKey(getMonitorItemsRequest.SecurityKey);

                    if (monitorAgent == null)
                    {
                        getMonitorItemsResponse.Response.ErrorCode = ResponseErrorCodes.PermissionDenied;
                        getMonitorItemsResponse.Response.ErrorMessage = EnumUtilities.GetEnumDescription(getMonitorItemsResponse.Response.ErrorCode);
                    }
                    else
                    {
                        var monitorItemService = scope.ServiceProvider.GetRequiredService<IMonitorItemService>();

                        getMonitorItemsResponse.MonitorItems = monitorItemService.GetAll();
                    }

                    // Send response
                    _connection.SendMessage(_messageConverters.GetMonitorItemsResponseConverter.GetConnectionMessage(getMonitorItemsResponse), messageReceivedInfo.RemoteEndpointInfo);

                    var error = getMonitorItemsResponse.Response.ErrorCode == null ? "Success" : getMonitorItemsResponse.Response.ErrorMessage;
                    _log.Log(DateTimeOffset.UtcNow, "Information", $"Processed {getMonitorItemsRequest.TypeId} from Monitor Agent {getMonitorItemsRequest.SenderAgentId}: {error}");
                }                
            });
        }

        private Task HandleGetSystemValueTypesRequestAsync(GetSystemValueTypesRequest getSystemValueTypesRequest, MessageReceivedInfo messageReceivedInfo)
        {
            return Task.Factory.StartNew(() =>
            {
                _log.Log(DateTimeOffset.UtcNow, "Information", $"Processing {getSystemValueTypesRequest.TypeId} from Monitor Agent {getSystemValueTypesRequest.SenderAgentId}");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var getSystemValueTypesResponse = new GetSystemValueTypesResponse()
                    {
                        Id = Guid.NewGuid().ToString(),                        
                        Response = new MessageResponse()
                        {
                            IsMore = false,
                            MessageId = getSystemValueTypesRequest.Id,
                            Sequence = 1
                        }
                    };

                    var monitorAgent = GetMonitorAgentBySecurityKey(getSystemValueTypesRequest.SecurityKey);

                    if (monitorAgent == null)
                    {
                        getSystemValueTypesResponse.Response.ErrorCode = ResponseErrorCodes.PermissionDenied;
                        getSystemValueTypesResponse.Response.ErrorMessage = EnumUtilities.GetEnumDescription(getSystemValueTypesResponse.Response.ErrorCode);
                    }
                    else
                    {
                        var systemValueTypeService = scope.ServiceProvider.GetRequiredService<ISystemValueTypeService>();

                        getSystemValueTypesResponse.SystemValueTypes = systemValueTypeService.GetAll();
                    }

                    // Send response
                    _connection.SendMessage(_messageConverters.GetSystemValueTypesResponseConverter.GetConnectionMessage(getSystemValueTypesResponse), messageReceivedInfo.RemoteEndpointInfo);

                    var error = getSystemValueTypesResponse.Response.ErrorCode == null ? "Success" : getSystemValueTypesResponse.Response.ErrorMessage;
                    _log.Log(DateTimeOffset.UtcNow, "Information", $"Processed {getSystemValueTypesRequest.TypeId} from Monitor Agent {getSystemValueTypesRequest.SenderAgentId}: {error}");
                }                
            });
        }

        /// <summary>
        /// Processes Monitor Agent heartbeat. If first heartbeat then creates MonitorAgent in DB.
        /// </summary>
        /// <param name="heartbeat"></param>
        /// <param name="messageReceivedInfo"></param>
        /// <returns></returns>
        private Task HandleHeartbeatAsync(Heartbeat heartbeat, MessageReceivedInfo messageReceivedInfo)
        {
            return Task.Factory.StartNew(() =>
            {
                _log.Log(DateTimeOffset.UtcNow, "Information", $"Processing {heartbeat.TypeId} from Monitor Agent {heartbeat.SenderAgentId}");
                
                using (var scope = _serviceProvider.CreateScope())
                {
                    var monitorAgentCheck = GetMonitorAgentBySecurityKey(heartbeat.SecurityKey);

                    if (monitorAgentCheck == null)   // No action, just ignore
                    {
                        //getSystemValueTypesResponse.Response.ErrorCode = ResponseErrorCodes.PermissionDenied;
                        //getSystemValueTypesResponse.Response.ErrorMessage = EnumUtilities.GetEnumDescription(getSystemValueTypesResponse.Response.ErrorCode);
                    }
                    else
                    {
                        var monitorAgentGroupService = scope.ServiceProvider.GetRequiredService<IMonitorAgentGroupService>();
                        var monitorAgentService = scope.ServiceProvider.GetRequiredService<IMonitorAgentService>();

                        // Get monitor agent                
                        var monitorAgent = monitorAgentService.GetByIdAsync(heartbeat.SenderAgentId).Result;
                        if (monitorAgent != null)       // Known monitor agent
                        {
                            monitorAgent.HeartbeatDateTime = DateTimeOffset.UtcNow;
                            monitorAgent.MachineName = heartbeat.MachineName;
                            monitorAgent.UserName = heartbeat.UserName;
                            monitorAgent.IP = messageReceivedInfo.RemoteEndpointInfo.Ip;
                            monitorAgent.Port = messageReceivedInfo.RemoteEndpointInfo.Port;
                            monitorAgent.Version = heartbeat.Version;

                            monitorAgentService.UpdateAsync(monitorAgent).Wait();
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
                                Version = heartbeat.Version
                            };

                            monitorAgentService.AddAsync(monitorAgent).Wait();
                        }
                    }
                }

                _log.Log(DateTimeOffset.UtcNow, "Information", $"Processed {heartbeat.TypeId} from Monitor Agent {heartbeat.SenderAgentId}");
            });
        }

        /// <summary>
        /// Processes monitor item result. Saves MonitorItemOutput and then executes action(s)
        /// </summary>
        /// <param name="monitorItemResultMessage"></param>
        /// <param name="messageReceivedInfo"></param>
        /// <returns></returns>
        private Task HandleMonitorItemResultAsync(MonitorItemResultMessage monitorItemResultMessage, MessageReceivedInfo messageReceivedInfo)
        {
            return Task.Factory.StartNew(() =>
            {
                _log.Log(DateTimeOffset.UtcNow, "Information", $"Processing {monitorItemResultMessage.TypeId} from Monitor Agent {monitorItemResultMessage.SenderAgentId}");

                var monitorAgentCheck = GetMonitorAgentBySecurityKey(monitorItemResultMessage.SecurityKey);

                if (monitorAgentCheck != null)
                {
                    if (monitorItemResultMessage.MonitorItemOutput != null)
                    {
                        // Save monitor item output
                        var monitorItemOutput = monitorItemResultMessage.MonitorItemOutput;
                        _monitorItemOutputService.AddAsync(monitorItemOutput).Wait();

                        // Get system user
                        var systemUser = _userService.GetAll().First(u => u.GetUserType() == UserTypes.System);

                        // Add "Checked monitor item" audit event
                        _auditEventService.AddAsync(_auditEventFactory.CreateCheckedMonitorItem(systemUser.Id, monitorItemOutput.Id)).Wait();

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
                                var monitorItem = monitorItemService.GetByIdAsync(monitorItemOutput.MonitorItemId).Result;

                                foreach (var eventItemId in monitorItemOutput.EventItemIdsForAction)
                                {
                                    // Get event item
                                    var eventItem = _eventItemService.GetByIdAsync(eventItemId).Result;

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
                                                auditEventService.AddAsync(auditEventFactory.CreateActionExecuted(systemUser.Id, monitorItemOutput.Id, actionItemId)).Wait();
                                            }
                                            else    // Add error audit event
                                            {
                                                // Note: MonitorItemOutput indicates MonitorItemId & MonitorAgentId
                                                auditEventService.AddAsync(auditEventFactory.CreateError(systemUser.Id, $"Error executing action item: {actionTasksNByActionItemId[actionItemId].Exception.Message}",
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
                                                                })).Wait();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                _log.Log(DateTimeOffset.UtcNow, "Information", $"Processed {monitorItemResultMessage.TypeId} from Monitor Agent {monitorItemResultMessage.SenderAgentId}");
            });
        }

        ///// <summary>
        ///// Waits for all responses for request until completed or timeout. Where multiple responses are required then
        ///// MessageBase.Response.IsMore=true for all except the last one.
        ///// </summary>
        ///// <param name="request">Request to check</param>
        ///// <param name="timeout">Timeout receiving responses</param>
        ///// <param name="responseMessagesToCheck">List where responses are added</param>
        ///// <param name="responseMessageAction">Action to forward next response</param>
        ///// <returns>Whether all responses received</returns>
        //private bool WaitForResponses(MessageBase request, TimeSpan timeout,
        //                              List<MessageBase> responseMessagesToCheck,
        //                              Action<MessageBase> responseMessageAction)
        //{
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();

        //    var isGotAllResponses = false;
        //    while (!isGotAllResponses &&
        //            stopwatch.Elapsed < timeout)
        //    {
        //        // Check for next response message
        //        var responseMessage = responseMessagesToCheck.FirstOrDefault(m => m.Response != null && m.Response.MessageId == request.Id);

        //        if (responseMessage != null)
        //        {
        //            // Discard
        //            responseMessagesToCheck.Remove(responseMessage);

        //            // Check if last response
        //            isGotAllResponses = !responseMessage.Response.IsMore;

        //            // Pass response to caller
        //            responseMessageAction(responseMessage);
        //        }

        //        Thread.Sleep(20);
        //    }

        //    return isGotAllResponses;
        //}
    }
}
