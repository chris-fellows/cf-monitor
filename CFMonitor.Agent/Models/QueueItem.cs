using CFConnectionMessaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Agent.Models
{
    public class QueueItem
    {
        public ConnectionMessage? ConnectionMessage { get; set; }

        public MessageReceivedInfo? MessageReceivedInfo { get; set; }
    }
}
