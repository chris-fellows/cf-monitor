using CFConnectionMessaging.Interfaces;
using CFConnectionMessaging.Models;
using CFMonitor.Constants;
using CFMonitor.Models.Messages;
using CFMonitor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.MessageConverters
{
    public class GetFileObjectResponseConverter : IExternalMessageConverter<GetFileObjectResponse>
    {
        public ConnectionMessage GetConnectionMessage(GetFileObjectResponse externalMessage)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = externalMessage.Id,
                TypeId = MessageTypeIds.GetFileObjectResponse,
                Parameters = new List<ConnectionMessageParameter>()
                {
                    new ConnectionMessageParameter()
                    {
                        Name = "Response",
                        Value = externalMessage.Response == null ? "" :
                                    JsonUtilities.SerializeToBase64String(externalMessage.Response,
                                            JsonUtilities.DefaultJsonSerializerOptions)
                    },
                }
            };
            return connectionMessage;
        }

        public GetFileObjectResponse GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var externalMessage = new GetFileObjectResponse()
            {
                Id = connectionMessage.Id,
            };

            return externalMessage;
        }
    }
}
