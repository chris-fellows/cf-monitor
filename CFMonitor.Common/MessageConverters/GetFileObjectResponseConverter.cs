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
    public class GetFileObjectResponseConverter : IExternalMessageConverter<GetFileObjectResponse>
    {
        public ConnectionMessage GetConnectionMessage(GetFileObjectResponse getFileObjectResponse)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = getFileObjectResponse.Id,
                TypeId = MessageTypeIds.GetFileObjectResponse,
                Parameters = new List<ConnectionMessageParameter>()
                {

                }
            };
            return connectionMessage;
        }

        public GetFileObjectResponse GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var getFileObjectResponse = new GetFileObjectResponse()
            {
                Id = connectionMessage.Id,
            };

            return getFileObjectResponse;
        }
    }
}
