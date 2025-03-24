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
    public class MonitorItemResultMessageConverter : IExternalMessageConverter<MonitorItemResultMessage>
    {
        public ConnectionMessage GetConnectionMessage(MonitorItemResultMessage monitorItemResultMessage)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = monitorItemResultMessage.Id,
                TypeId = MessageTypeIds.MonitorItemUpdated,
                Parameters = new List<ConnectionMessageParameter>()
                {

                }
            };
            return connectionMessage;
        }

        public MonitorItemResultMessage GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var monitorItemResultMessage = new MonitorItemResultMessage()
            {
                Id = connectionMessage.Id,
            };

            return monitorItemResultMessage;
        }
    }
}
