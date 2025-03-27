using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using CFWebServer.Services;

namespace CFMonitor.Services
{
    public class XmlSystemTaskJobService : XmlEntityWithIdService<SystemTaskJob, string>, ISystemTaskJobService
    {
        public XmlSystemTaskJobService(string folder) : base(folder,
                                                "SystemTaskJob.*.xml",
                                              (systemTaskJob) => $"SystemTaskJob.{systemTaskJob.Id}.xml",
                                                (systemTaskJobId) => $"SystemTaskJob.{systemTaskJobId}.xml")
        {

        }

        public List<SystemTaskJob> GetByFilter(SystemTaskJobFilter filter)
        {
            var systemTaskJobs = GetAll()
                            .Where
                            (i =>
                                 (
                                     filter.CreatedDateTimeFrom == null ||
                                     i.CreatedDateTime >= filter.CreatedDateTimeFrom
                                 ) &&
                                 (
                                     filter.CreatedDateTimeTo == null ||
                                     i.CreatedDateTime <= filter.CreatedDateTimeTo
                                 ) &&
                                 (
                                     filter.TypeIds == null ||
                                     !filter.TypeIds.Any() ||
                                     filter.TypeIds.Contains(i.TypeId)
                                 ) &&
                                 (
                                     filter.StatusIds == null ||
                                     !filter.StatusIds.Any() ||
                                     filter.StatusIds.Contains(i.StatusId)
                                 )
                         ).ToList();

            // Filter on parameters
            if (filter.Parameters != null && filter.Parameters.Any())
            {
                switch (filter.ParametersLogicalOperator)
                {
                    case LogicalOperators.And:    // Jobs containing all filter parameter values
                        systemTaskJobs = systemTaskJobs.Where(j => filter.Parameters.All(fp =>
                              j.Parameters.Any(jp =>
                                      jp.SystemValueTypeId == fp.TypeId &&
                                      fp.Values.Contains(jp.Value))
                              )).ToList();
                        break;
                    case LogicalOperators.Or:   // Jobs containing any filter parameter values
                        systemTaskJobs = systemTaskJobs.Where(j => filter.Parameters.Any(fp =>
                            j.Parameters.Any(jp =>
                                    jp.SystemValueTypeId == fp.TypeId &&
                                    fp.Values.Contains(jp.Value))
                            )).ToList();
                        break;
                }
            }

            return systemTaskJobs;
        }
    }
}
