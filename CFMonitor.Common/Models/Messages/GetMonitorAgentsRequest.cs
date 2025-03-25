using CFMonitor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Models.Messages
{
    public class GetMonitorAgentsRequest : MessageBase
    {
        public GetMonitorAgentsRequest()
        {
            Id = Guid.NewGuid().ToString();
            TypeId = MessageTypeIds.GetMonitorAgentsRequest;
        }
    }
}
