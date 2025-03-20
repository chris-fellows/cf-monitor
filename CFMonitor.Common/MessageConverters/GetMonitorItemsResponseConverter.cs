using CFConnectionMessaging.Interfaces;
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
    public class GetMonitorItemsResponseConverter : IExternalMessageConverter<GetMonitorItemsResponse>
    {
        public ConnectionMessage GetConnectionMessage(GetMonitorItemsResponse response)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = response.Id,
                TypeId = MessageTypeIds.GetMonitorItemsResponse,
                Parameters = new List<ConnectionMessageParameter>()
                {

                }
            };
            return connectionMessage;
        }

        public GetMonitorItemsResponse GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var response = new GetMonitorItemsResponse()
            {
                Id = connectionMessage.Id,
            };

            return response;
        }
    }
}
