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

        /// <summary>
        /// Root folder where files used for checks are stored. Typically in a sub-folder below the
        /// Monitor Agent EXE.
        /// </summary>
        public string FilesRootFolder { get; set; } = String.Empty;
    }
}
