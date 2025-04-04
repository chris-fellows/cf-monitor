﻿using CFConnectionMessaging.Interfaces;
using CFConnectionMessaging.Models;
using CFMonitor.Constants;
using CFMonitor.Models;
using CFMonitor.Models.Messages;
using CFMonitor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.MessageConverters
{
    public class GetMonitorItemsResponseConverter : IExternalMessageConverter<GetMonitorItemsResponse>
    {
        public ConnectionMessage GetConnectionMessage(GetMonitorItemsResponse externalMessage)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = externalMessage.Id,
                TypeId = MessageTypeIds.GetMonitorItemsResponse,
                Parameters = new List<ConnectionMessageParameter>()
                {
                     new ConnectionMessageParameter()
                    {
                        Name = "Response",
                        Value = externalMessage.Response == null ? "" :
                                    JsonUtilities.SerializeToBase64String(externalMessage.Response,
                                            JsonUtilities.DefaultJsonSerializerOptions)
                    },
                   new ConnectionMessageParameter()
                   {
                       Name = "MonitorItems",
                       Value = externalMessage.MonitorItems == null ? "" :
                                        JsonUtilities.SerializeToBase64String(externalMessage.MonitorItems,
                                        JsonUtilities.DefaultJsonSerializerOptions)
                   }
                }
            };
            return connectionMessage;
        }

        public GetMonitorItemsResponse GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var externalMessage = new GetMonitorItemsResponse()
            {
                Id = connectionMessage.Id,
            };

            // Get response
            var responseParameter = connectionMessage.Parameters.First(p => p.Name == "Response");
            if (!String.IsNullOrEmpty(responseParameter.Value))
            {
                externalMessage.Response = JsonUtilities.DeserializeFromBase64String<MessageResponse>(responseParameter.Value, JsonUtilities.DefaultJsonSerializerOptions);
            }

            // Get monitor agents
            var monitorItemsParameter = connectionMessage.Parameters.First(p => p.Name == "MonitorItems");
            if (!String.IsNullOrEmpty(monitorItemsParameter.Value))
            {
                externalMessage.MonitorItems = JsonUtilities.DeserializeFromBase64String<List<MonitorItem>>(monitorItemsParameter.Value, JsonUtilities.DefaultJsonSerializerOptions);
            }

            return externalMessage;
        }
    }
}
