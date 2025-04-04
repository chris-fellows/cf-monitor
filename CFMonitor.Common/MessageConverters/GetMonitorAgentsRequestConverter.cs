﻿using CFConnectionMessaging.Interfaces;
using CFConnectionMessaging.Models;
using CFMonitor.Constants;
using CFMonitor.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.MessageConverters
{
    public class GetMonitorAgentsRequestConverter : IExternalMessageConverter<GetMonitorAgentsRequest>
    {
        public ConnectionMessage GetConnectionMessage(GetMonitorAgentsRequest externalMessage)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = externalMessage.Id,
                TypeId = MessageTypeIds.GetMonitorAgentsRequest,
                Parameters = new List<ConnectionMessageParameter>()
                {
                       new ConnectionMessageParameter()
                    {
                        Name = "SecurityKey",
                        Value = externalMessage.SecurityKey
                    },
                    new ConnectionMessageParameter()
                    {
                        Name = "SenderAgentId",
                        Value = externalMessage.SenderAgentId
                    }
                }
            };
            return connectionMessage;
        }

        public GetMonitorAgentsRequest GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var getEventItemsRequest = new GetMonitorAgentsRequest()
            {
                Id = connectionMessage.Id,
                SecurityKey = connectionMessage.Parameters.First(p => p.Name == "SecurityKey").Value,
                SenderAgentId = connectionMessage.Parameters.First(p => p.Name == "SenderAgentId").Value
            };

            return getEventItemsRequest;
        }
    }
}
