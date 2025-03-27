using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Interfaces
{
    public interface ISystemTaskJobService : IEntityWithIdService<SystemTaskJob, string>
    {
        List<SystemTaskJob> GetByFilter(SystemTaskJobFilter filter);
    }
}
