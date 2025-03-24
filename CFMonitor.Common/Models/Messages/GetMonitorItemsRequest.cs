using CFMonitor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Models.Messages
{
    /// <summary>
    /// Request for monitor items
    /// </summary>
    public class GetMonitorItemsRequest : MessageBase
    {
        public GetMonitorItemsRequest()
        {
            Id = Guid.NewGuid().ToString();
            TypeId = MessageTypeIds.GetMonitorItemsRequest;
        }
    }
}
