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
    public class MonitorAgentLogMessageConverter : IExternalMessageConverter<MonitorAgentLogMessage>
    {
        public ConnectionMessage GetConnectionMessage(MonitorAgentLogMessage externalMessage)
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
                       Name = "FileName",
                       Value = externalMessage.FileName
                   },
                   new ConnectionMessageParameter()
                   {
                       Name = "Content",
                       Value = externalMessage.Content.Length == 0 ? "" : Convert.ToBase64String(externalMessage.Content)
                   }
                }
            };
            return connectionMessage;
        }

        public MonitorAgentLogMessage GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var externalMessage = new MonitorAgentLogMessage()
            {
                Id = connectionMessage.Id,
                SecurityKey = connectionMessage.Parameters.First(p => p.Name == "SecurityKey").Value,
                SenderAgentId = connectionMessage.Parameters.First(p => p.Name == "SenderAgentId").Value,
                FileName = connectionMessage.Parameters.First(p => p.Name == "FileName").Value,
                Content = new byte[0],                
            };

            // Get monitor item output
            var contentParameter = connectionMessage.Parameters.First(p => p.Name == "Content");
            if (!String.IsNullOrEmpty(contentParameter.Value))
            {
                externalMessage.Content = Convert.FromBase64String(contentParameter.Value);
            }

            return externalMessage;
        }
    }
}
