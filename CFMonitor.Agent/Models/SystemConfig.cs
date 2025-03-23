using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Agent.Models
{
    public class SystemConfig
    {
        public int LocalPort { get; set; }
        
        public int MaxConcurrentChecks { get; set; }
    }
}
