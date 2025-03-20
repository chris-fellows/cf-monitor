using CFMonitor.Models.Messages;
using CFConnectionMessaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFConnectionMessaging.Models;
using CFMonitor.Constants;

namespace CFMonitor.MessageConverters
{
    public class GetFileObjectRequestConverter : IExternalMessageConverter<GetFileObjectRequest>
    {
        public ConnectionMessage GetConnectionMessage(GetFileObjectRequest monitorItemUpdated)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = monitorItemUpdated.Id,
                TypeId = MessageTypeIds.GetFileObjectRequest,
                Parameters = new List<ConnectionMessageParameter>()
                {

                }
            };
            return connectionMessage;
        }

        public GetFileObjectRequest GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var getFileObjectRequest = new GetFileObjectRequest()
            {
                Id = connectionMessage.Id,
            };

            return getFileObjectRequest;
        }
    }
}
