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
    public class GetEventItemsResponseConverter : IExternalMessageConverter<GetEventItemsResponse>
    {
        public ConnectionMessage GetConnectionMessage(GetEventItemsResponse externalMessage)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = externalMessage.Id,
                TypeId = MessageTypeIds.GetEventItemsResponse,
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
                       Name = "EventItems",                       
                       Value = externalMessage.EventItems == null ? "" :
                                        JsonUtilities.SerializeToBase64String(externalMessage.EventItems,
                                        JsonUtilities.DefaultJsonSerializerOptions)
                   }
                }
            };
            return connectionMessage;
        }

        public GetEventItemsResponse GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var externalMessage = new GetEventItemsResponse()
            {
                Id = connectionMessage.Id,
            };

            // Get response
            var responseParameter = connectionMessage.Parameters.First(p => p.Name == "Response");
            if (!String.IsNullOrEmpty(responseParameter.Value))
            {
                externalMessage.Response = JsonUtilities.DeserializeFromBase64String<MessageResponse>(responseParameter.Value, JsonUtilities.DefaultJsonSerializerOptions);
            }

            // Get event items
            var eventItemsParameter = connectionMessage.Parameters.First(p => p.Name == "EventItems");
            if (!String.IsNullOrEmpty(eventItemsParameter.Value))
            {
                externalMessage.EventItems = JsonUtilities.DeserializeFromBase64String<List<EventItem>>(eventItemsParameter.Value, JsonUtilities.DefaultJsonSerializerOptions);                
            }


            return externalMessage;
        }
    }
}
