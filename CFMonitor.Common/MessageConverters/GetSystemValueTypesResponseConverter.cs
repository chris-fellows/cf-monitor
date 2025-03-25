using CFConnectionMessaging.Interfaces;
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
    public class GetSystemValueTypesResponseConverter : IExternalMessageConverter<GetSystemValueTypesResponse>
    {
        public ConnectionMessage GetConnectionMessage(GetSystemValueTypesResponse externalMessage)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = externalMessage.Id,
                TypeId = MessageTypeIds.GetSystemValueTypesResponse,
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
                       Name = "SystemValueTypes",
                       Value = externalMessage.SystemValueTypes == null ? "" :
                                        JsonUtilities.SerializeToBase64String(externalMessage.SystemValueTypes,
                                        JsonUtilities.DefaultJsonSerializerOptions)
                   }
                }
            };
            return connectionMessage;
        }

        public GetSystemValueTypesResponse GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var externalMessage = new GetSystemValueTypesResponse()
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
            var systemValueTypesParameter = connectionMessage.Parameters.First(p => p.Name == "SystemValueTypes");
            if (!String.IsNullOrEmpty(systemValueTypesParameter.Value))
            {
                externalMessage.SystemValueTypes = JsonUtilities.DeserializeFromBase64String<List<SystemValueType>>(systemValueTypesParameter.Value, JsonUtilities.DefaultJsonSerializerOptions);
            }

            return externalMessage;
        }
    }
}
