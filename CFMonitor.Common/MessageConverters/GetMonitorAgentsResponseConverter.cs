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
    public class GetMonitorAgentsResponseConverter : IExternalMessageConverter<GetMonitorAgentsResponse>
    {
        public ConnectionMessage GetConnectionMessage(GetMonitorAgentsResponse externalMessage)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = externalMessage.Id,
                TypeId = MessageTypeIds.GetMonitorAgentsResponse,
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
                       Name = "MonitorAgents",
                       Value = externalMessage.MonitorAgents == null ? "" :
                                        JsonUtilities.SerializeToBase64String(externalMessage.MonitorAgents,
                                        JsonUtilities.DefaultJsonSerializerOptions)
                   }
                }
            };
            return connectionMessage;
        }

        public GetMonitorAgentsResponse GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var externalMessage = new GetMonitorAgentsResponse()
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
            var monitorAgentsParameter = connectionMessage.Parameters.First(p => p.Name == "MonitorAgents");
            if (!String.IsNullOrEmpty(monitorAgentsParameter.Value))
            {
                externalMessage.MonitorAgents = JsonUtilities.DeserializeFromBase64String<List<MonitorAgent>>(monitorAgentsParameter.Value, JsonUtilities.DefaultJsonSerializerOptions);
            }

            return externalMessage;
        }
    }
}
