using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Enums
{
    public enum  ActionerTypes : byte
    {
        DatadogWarning,
        Email,
        Log,
        Process,
        SMS,
        SQL,
        URL
    }
}
