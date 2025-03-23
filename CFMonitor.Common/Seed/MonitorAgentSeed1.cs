using CFMonitor.EntityReader;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Seed
{
    public class MonitorAgentSeed1 : IEntityReader<MonitorAgent>
    {
        public IEnumerable<MonitorAgent> Read()
        {
            var items = new List<MonitorAgent>()
            {
                new MonitorAgent()
                {
                    Id = Guid.NewGuid().ToString(),
                     MachineName  = Environment.MachineName,
                      UserName = Environment.UserName
                },
                       new MonitorAgent()
                {
                    Id = Guid.NewGuid().ToString(),
                     MachineName  = Environment.MachineName + "XXX",
                      UserName = Environment.UserName
                }
            };

            return items;
        }
    }
}
