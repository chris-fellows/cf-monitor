using CFConnectionMessaging.Interfaces;
using CFConnectionMessaging.Models;
using CFMonitor.Constants;
using CFMonitor.Models.Messages;

namespace CFMonitor.MessageConverters
{
    public class GetMonitorItemsRequestConverter : IExternalMessageConverter<GetMonitorItemsRequest>
    {
        public ConnectionMessage GetConnectionMessage(GetMonitorItemsRequest request)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = request.Id,
                TypeId = MessageTypeIds.GetMonitorItemsRequest,
                Parameters = new List<ConnectionMessageParameter>()
                {
                   
                }
            };
            return connectionMessage;
        }

        public GetMonitorItemsRequest GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var request = new GetMonitorItemsRequest()
            {
                Id = connectionMessage.Id,              
            };

            return request;
        }
    }
}
