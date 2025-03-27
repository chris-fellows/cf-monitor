using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFWebServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Services
{
    public class XmlMonitorItemOutputService : XmlEntityWithIdService<MonitorItemOutput, string>, IMonitorItemOutputService
    {
        public XmlMonitorItemOutputService(string folder) : base(folder,
                                                "MonitorItemOutput.*.xml",
                                              (monitorItemOutput) => $"MonitorItemOutput.{monitorItemOutput.Id}.xml",
                                                (monitorItemOutputId) => $"MonitorItemOutput.{monitorItemOutputId}.xml")
        {

        }

        public List<MonitorItemOutput> GetByFilter(MonitorItemOutputFilter filter)
        {
            var monitorItemOutputs = GetAll()
                        .Where(i =>
                        (
                           (
                               filter.CheckedDateTimeFrom == null ||
                               i.CheckedDateTime >= filter.CheckedDateTimeFrom
                           ) &&
                           (
                               filter.CheckedDateTimeTo == null ||
                               i.CheckedDateTime <= filter.CheckedDateTimeTo
                           ) &&
                           (
                               filter.MonitorAgentIds == null ||
                               !filter.MonitorAgentIds.Any() ||
                               filter.MonitorAgentIds.Contains(i.MonitorAgentId)
                           ) &&
                           (
                               filter.MonitorItemIds == null ||
                               !filter.MonitorItemIds.Any() ||
                               filter.MonitorItemIds.Contains(i.MonitorItemId)
                           )
                        )).ToList();

            if (filter.LatestOnly)
            {                
                var newMonitorItemOutputs = new List<MonitorItemOutput>();
                var monitorAgentIds = monitorItemOutputs.Select(mio => mio.MonitorAgentId).Distinct().ToList();

                foreach (var monitorItemId in monitorItemOutputs.Select(mio => mio.MonitorItemId).Distinct())
                {                    
                    foreach(var monitorAgentId in monitorAgentIds)
                    {
                        var monitorItemOutput = monitorItemOutputs.Where(mio => mio.MonitorItemId == monitorItemId)
                                            .Where(mio => mio.MonitorAgentId == monitorAgentId)
                                            .OrderBy(mio => mio.CheckedDateTime).LastOrDefault();

                        if (monitorItemOutput != null) newMonitorItemOutputs.Add(monitorItemOutput);

                    }                    
                }

                monitorItemOutputs.Clear();
                monitorItemOutputs.AddRange(newMonitorItemOutputs);
            }

            return monitorItemOutputs;
        }
    }
}
