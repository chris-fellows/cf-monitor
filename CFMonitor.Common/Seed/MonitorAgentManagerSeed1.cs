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
    public class MonitorAgentManagerSeed1 : IEntityReader<MonitorAgentManager>
    {
        public IEnumerable<MonitorAgentManager> Read()
        {                        
            var items = new List<MonitorAgentManager>()
            {
                new MonitorAgentManager()
                {
                    Id = Guid.NewGuid().ToString(),                                    
                    MachineName  = Environment.MachineName                    
                },
                new MonitorAgentManager()
                {
                    Id = Guid.NewGuid().ToString(),            
                    MachineName  = Environment.MachineName + "XXX"                    
                }
            };

            return items;
        }
    }
}
