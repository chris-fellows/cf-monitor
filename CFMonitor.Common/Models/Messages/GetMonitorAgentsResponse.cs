using CFMonitor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Models.Messages
{
    public class GetMonitorAgentsResponse : MessageBase
    {
        public List<MonitorAgent> MonitorAgents { get; set; } = new();

        public GetMonitorAgentsResponse()
        {
            Id = Guid.NewGuid().ToString();
            TypeId = MessageTypeIds.GetMonitorAgentsResponse;
        }
    }
}
