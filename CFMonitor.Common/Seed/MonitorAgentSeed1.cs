﻿using CFMonitor.EntityReader;
using CFMonitor.Interfaces;
using CFMonitor.Models;

namespace CFMonitor.Seed
{
    public class MonitorAgentSeed1 : IEntityReader<MonitorAgent>
    {
        private readonly IMonitorAgentGroupService _monitorAgentGroupService;

        public MonitorAgentSeed1(IMonitorAgentGroupService monitorAgentGroupService)
        {
            _monitorAgentGroupService = monitorAgentGroupService;
        }

        public IEnumerable<MonitorAgent> Read()
        {
            // Default to first group (Same as heartbeat)
            var monitorAgentGroup = _monitorAgentGroupService.GetAll().OrderBy(g => g.Name).First();
            
            // IP & Port will be set when Monitor Agent sends heartbeat
            var items = new List<MonitorAgent>()
            {
                new MonitorAgent()
                {
                    Id = Guid.NewGuid().ToString(),
                    MonitorAgentGroupId = monitorAgentGroup.Id,
                    MachineName  = Environment.MachineName,
                    SecurityKey = "111111"
                },
                new MonitorAgent()
                {
                    Id = Guid.NewGuid().ToString(),
                    MonitorAgentGroupId = monitorAgentGroup.Id,
                    MachineName  = Environment.MachineName + "XXX",
                    SecurityKey = "222222"
                }
            };

            return items;
        }
    }
}
