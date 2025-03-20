using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Seed
{
    public class MonitorAgentSeed1 : IEntityReader<MonitorAgent>
    {
        public Task<List<MonitorAgent>> ReadAllAsync()
        {
            var items = new List<MonitorAgent>()
            {
                new MonitorAgent()
                {
                    ID = Guid.NewGuid().ToString(),
                     MachineName  = Environment.MachineName,
                      UserName = Environment.UserName
                },
                       new MonitorAgent()
                {
                    ID = Guid.NewGuid().ToString(),
                     MachineName  = Environment.MachineName + "XXX",
                      UserName = Environment.UserName
                }
            };

            return Task.FromResult(items);
        }

    }
}
