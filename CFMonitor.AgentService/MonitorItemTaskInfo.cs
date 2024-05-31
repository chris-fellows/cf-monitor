using CFMonitor.Models.MonitorItems;
using System;
using System.Threading.Tasks;

namespace CFMonitor.AgentService
{
    /// <summary>
    /// Details of active monitor item task
    /// </summary>
    internal class MonitorItemTaskInfo
    {
        public MonitorItem MonitorItem { get; set; }

        public DateTime TimeStarted { get; set; }

        public Task Task { get; set; }
    }
}
