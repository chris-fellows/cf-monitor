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
        public ConnectionMessage GetConnectionMessage(Heartbeat heartbeat)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = heartbeat.Id,
                TypeId = MessageTypeIds.Heartbeat,
                Parameters = new List<ConnectionMessageParameter>()
                {

                }
            };
            return connectionMessage;
        }

        public Heartbeat GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var heartbeat = new Heartbeat()
            {
                Id = connectionMessage.Id,
            };

            return heartbeat;
        }
    }
}
