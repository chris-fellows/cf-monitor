using CFMonitor.Constants;
using CFMonitor.EntityReader;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Seed
{
    public class SystemTaskStatusSeed1 : IEntityReader<SystemTaskStatus>
    {
        public IEnumerable<SystemTaskStatus> Read()
        {
            var list = new List<SystemTaskStatus>()
            {
                new SystemTaskStatus()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemTaskStatusNames.Pending
                },
                new SystemTaskStatus()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemTaskStatusNames.Active
                },
                new SystemTaskStatus()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemTaskStatusNames.CompletedSuccess
                },
                new SystemTaskStatus()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemTaskStatusNames.CompletedError
                },
                new SystemTaskStatus()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemTaskStatusNames.Cancelled
                }
            };

            return list;
        }
    }
}
