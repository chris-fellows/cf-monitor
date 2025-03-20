using CFMonitor.Constants;
using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Seed
{
    public class SystemValueTypeSeed1 : IEntityReader<SystemValueType>
    {
        public Task<List<SystemValueType>> ReadAllAsync()
        {
            var list = new List<SystemValueType>()
            {
                new SystemValueType()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Email body",
                    ValueType = SystemValueTypes.AIP_EmailBody
                }
            };

            return Task.FromResult(list);
        }
    }
}
