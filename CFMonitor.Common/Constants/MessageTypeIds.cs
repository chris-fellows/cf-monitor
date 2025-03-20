using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Constants
{
    public static class MessageTypeIds
    {
        public const string GetMonitorItemsRequest = "GetMonitorItemRequest";
        public const string GetMonitorItemsResponse = "GetMonitorItemResponse";
        public const string Heartbeat = "Heartbeat";
        public const string MonitorItemUpdated = "MonitorItemUpdated";
    }
}
