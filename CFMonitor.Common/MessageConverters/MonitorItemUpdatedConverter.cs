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
    public class MonitorItemUpdatedConverter : IExternalMessageConverter<MonitorItemUpdated>
    {
        public ConnectionMessage GetConnectionMessage(MonitorItemUpdated monitorItemUpdated)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = monitorItemUpdated.Id,
                TypeId = MessageTypeIds.MonitorItemUpdated,
                Parameters = new List<ConnectionMessageParameter>()
                {

                }
            };
            return connectionMessage;
        }

        public MonitorItemUpdated GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var monitorItemUpdated = new MonitorItemUpdated()
            {
                Id = connectionMessage.Id,
            };

            return monitorItemUpdated;
        }
    }
}
