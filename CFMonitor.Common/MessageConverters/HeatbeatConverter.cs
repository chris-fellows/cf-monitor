using CFConnectionMessaging.Interfaces;
using CFConnectionMessaging.Models;
using CFMonitor.Constants;
using CFMonitor.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Common.MessageConverters
{
    public class HeartbeatConverter : IExternalMessageConverter<Heartbeat>
    {
        public ConnectionMessage GetConnectionMessage(Heartbeat externalMessage)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = externalMessage.Id,
                TypeId = MessageTypeIds.Heartbeat,                
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

        public Heartbeat GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var heartbeat = new Heartbeat()
            {
                Id = connectionMessage.Id,
                SecurityKey = connectionMessage.Parameters.First(p => p.Name == "SecurityKey").Value,
                SenderAgentId = connectionMessage.Parameters.First(p => p.Name == "SenderAgentId").Value
            };

            return heartbeat;
        }
    }
}
