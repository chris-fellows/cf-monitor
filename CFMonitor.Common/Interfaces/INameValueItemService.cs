using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Common.Interfaces
{
    public interface INameValueItemService : IEntityWithIdAndNameService<NameValueItem, string>
    {
    }
}
