using CFMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Models
{
    public class SystemValueType
    {
        public string Id { get; set; } = String.Empty;

        public SystemValueTypes ValueType { get; set; }

        public string Name { get; set; } = String.Empty;
    }
}
