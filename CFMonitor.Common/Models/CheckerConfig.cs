using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Models
{
    public class CheckerConfig
    {
        public bool TestMode { get; set; }
        public string FilesRootFolder { get; set; } = String.Empty;
    }
}
