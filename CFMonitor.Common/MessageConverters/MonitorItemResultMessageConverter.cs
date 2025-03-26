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
    public class MonitorItemResultMessageConverter : IExternalMessageConverter<MonitorItemResultMessage>
    {
        public ConnectionMessage GetConnectionMessage(MonitorItemResultMessage externalMessage)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = externalMessage.Id,
                TypeId = MessageTypeIds.MonitorItemResultMessage,
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
                       Name = "MonitorItemOutput",
                       Value = externalMessage.MonitorItemOutput == null ? "" :
                                        JsonUtilities.SerializeToBase64String(externalMessage.MonitorItemOutput,
                                        JsonUtilities.DefaultJsonSerializerOptions)
                   }
                }
            };
            return connectionMessage;
        }

        public MonitorItemResultMessage GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var externalMessage = new MonitorItemResultMessage()
            {
                Id = connectionMessage.Id,
                SecurityKey = connectionMessage.Parameters.First(p => p.Name == "SecurityKey").Value,
                SenderAgentId = connectionMessage.Parameters.First(p => p.Name == "SenderAgentId").Value
            };

            // Get monitor item output
            var monitorItemOutputParameter = connectionMessage.Parameters.First(p => p.Name == "MonitorItemOutput");
            if (!String.IsNullOrEmpty(monitorItemOutputParameter.Value))
            {
                externalMessage.MonitorItemOutput = JsonUtilities.DeserializeFromBase64String<MonitorItemOutput>(monitorItemOutputParameter.Value, JsonUtilities.DefaultJsonSerializerOptions);
            }

            return externalMessage;
        }
    }
}
