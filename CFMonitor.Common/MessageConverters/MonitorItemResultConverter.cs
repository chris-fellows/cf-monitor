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
    public class MonitorItemResultConverter : IExternalMessageConverter<MonitorItemResult>
    {
        public ConnectionMessage GetConnectionMessage(MonitorItemResult monitorItemResult)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = monitorItemResult.Id,
                TypeId = MessageTypeIds.MonitorItemUpdated,
                Parameters = new List<ConnectionMessageParameter>()
                {

                }
            };
            return connectionMessage;
        }

        public MonitorItemResult GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var monitorItemResult = new MonitorItemResult()
            {
                Id = connectionMessage.Id,
            };

            return monitorItemResult;
        }
    }
}
