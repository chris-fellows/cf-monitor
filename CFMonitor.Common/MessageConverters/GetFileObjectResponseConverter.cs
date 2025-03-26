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
                    new ConnectionMessageParameter()
                   {
                       Name = "FileObject",
                       Value = externalMessage.FileObject == null ? "" :
                                        JsonUtilities.SerializeToBase64String(externalMessage.FileObject,
                                        JsonUtilities.DefaultJsonSerializerOptions)
                   }
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

            // Get response
            var responseParameter = connectionMessage.Parameters.First(p => p.Name == "Response");
            if (!String.IsNullOrEmpty(responseParameter.Value))
            {
                externalMessage.Response = JsonUtilities.DeserializeFromBase64String<MessageResponse>(responseParameter.Value, JsonUtilities.DefaultJsonSerializerOptions);
            }

            // Get file object
            var fileObjectParameter = connectionMessage.Parameters.First(p => p.Name == "FileObject");
            if (!String.IsNullOrEmpty(fileObjectParameter.Value))
            {
                externalMessage.FileObject = JsonUtilities.DeserializeFromBase64String<FileObject>(fileObjectParameter.Value, JsonUtilities.DefaultJsonSerializerOptions);
            }

            return externalMessage;
        }
    }
}
