using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor
{
    internal class Globals
    {
        public static string MonitorItemFolder
        {       
            get { return System.Configuration.ConfigurationSettings.AppSettings.Get("MonitorItemFolder"); }
        }
    }
}
