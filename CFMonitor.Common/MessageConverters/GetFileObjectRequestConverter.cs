using CFMonitor.Models.Messages;
using CFConnectionMessaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFConnectionMessaging.Models;
using CFMonitor.Constants;
using System.Collections.Frozen;

namespace CFMonitor.MessageConverters
{
    public class GetFileObjectRequestConverter : IExternalMessageConverter<GetFileObjectRequest>
    {
        public ConnectionMessage GetConnectionMessage(GetFileObjectRequest externalMessage)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = externalMessage.Id,
                TypeId = MessageTypeIds.GetFileObjectRequest,
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
                    },
                    new ConnectionMessageParameter()
                    {
                        Name = "FileObjectId",
                        Value = externalMessage.FileObjectId
                    }
                }
            };
            return connectionMessage;
        }

        public GetFileObjectRequest GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var getFileObjectRequest = new GetFileObjectRequest()
            {
                Id = connectionMessage.Id,
                SecurityKey = connectionMessage.Parameters.First(p => p.Name == "SecurityKey").Value,
                SenderAgentId = connectionMessage.Parameters.First(p => p.Name == "SenderAgentId").Value,
                FileObjectId = connectionMessage.Parameters.First(p => p.Name == "FileObjectId").Value
            };

            return getFileObjectRequest;
        }
    }
}
