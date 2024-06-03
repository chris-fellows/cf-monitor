using CFMonitor.Actioners;
using CFMonitor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Interfaces
{
    public interface IActionersService
    {
        List<IActioner> GetAll();
    }
}
