using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Models.Messages
{
    public class GetMonitorItemsResponse : MessageBase
    {
        public List<MonitorItem> MonitorItems { get; set; } = new();
    }
}
