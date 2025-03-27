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
    public class SystemTaskTypeSeed1 : IEntityReader<SystemTaskType>
    {
        public IEnumerable<SystemTaskType> Read()
        {
            var list = new List<SystemTaskType>()
            {
                new SystemTaskType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemTaskTypeNames.ManagePasswordResets
                },
                new SystemTaskType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemTaskTypeNames.ManageSystemTaskJobs
                },
                //   new SystemTaskType()
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    Name = SystemTaskTypeNames.SendDatadog
                //},
                new SystemTaskType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = SystemTaskTypeNames.SendEmail
                },
                //   new SystemTaskType()
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    Name = SystemTaskTypeNames.SendSlack
                //},
                //      new SystemTaskType()
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    Name = SystemTaskTypeNames.SendTeams
                //},
            };

            return list;
        }
    }
}
