using CFConnectionMessaging.Interfaces;
using CFConnectionMessaging.Models;
using CFMonitor.Constants;
using CFMonitor.Models.Messages;

namespace CFMonitor.MessageConverters
{
    public class GetMonitorItemsRequestConverter : IExternalMessageConverter<GetMonitorItemsRequest>
    {
        public ConnectionMessage GetConnectionMessage(GetMonitorItemsRequest externalMessage)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = externalMessage.Id,
                TypeId = MessageTypeIds.GetMonitorItemsRequest,
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

        public GetMonitorItemsRequest GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var request = new GetMonitorItemsRequest()
            {
                Id = connectionMessage.Id,
                SecurityKey = connectionMessage.Parameters.First(p => p.Name == "SecurityKey").Value,
                SenderAgentId = connectionMessage.Parameters.First(p => p.Name == "SenderAgentId").Value
            };

            return request;
        }
    }
}
