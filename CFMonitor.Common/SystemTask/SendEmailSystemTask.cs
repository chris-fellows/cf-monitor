using CFMonitor.Constants;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Common.SystemTask
{
    public class SendEmailSystemTask
    {
        public string Name => SystemTaskTypeNames.SendEmail;

        public async Task ExecuteAsync(Dictionary<string, object> parameters, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            //// Get services
            //var auditEventService = serviceProvider.GetRequiredService<IAuditEventService>();
            //var auditEventTypeService = serviceProvider.GetRequiredService<IAuditEventTypeService>();
            //var emailService = serviceProvider.GetRequiredService<IEmailService>();
            //var systemTaskJobService = serviceProvider.GetRequiredService<ISystemTaskJobService>();
            //var systemTaskStatusService = serviceProvider.GetRequiredService<ISystemTaskStatusService>();
            //var systemTaskTypeService = serviceProvider.GetRequiredService<ISystemTaskTypeService>();
            //var systemValueTypeService = serviceProvider.GetRequiredService<ISystemValueTypeService>();
            //var userService = serviceProvider.GetRequiredService<IUserService>();

            //// Set current user as System user
            //var currentUser = (userService.GetAll()).First(u => u.GetUserType() == Enums.UserTypes.System);

            //// Get system task statuses
            //var systemTaskStatusPending = systemTaskStatusService.GetByName(SystemTaskStatusNames.Pending);
            //var systemTaskStatusActive = systemTaskStatusService.GetByName(SystemTaskStatusNames.Active);
            //var systemTaskStatusCompletedSuccess =  systemTaskStatusService.GetByName(SystemTaskStatusNames.CompletedSuccess);
            //var systemTaskStatusCompletedError = systemTaskStatusService.GetByName(SystemTaskStatusNames.CompletedError);

            //// Get system task type
            //var systemTaskTypeSend = systemTaskTypeService.GetByName(SystemTaskTypeNames.SendEmail);

            //// Get value type for email creator
            //var systemValueTypeCreator =  systemValueTypeService.GetByName(SystemValueTypeNames.EmailCreator);

            //// Get system task jobs for Pending status
            //var systemTaskJobFilter = new SystemTaskJobFilter()
            //{
            //    StatusIds = new List<string>() { systemTaskStatusPending.Id },
            //    TypeIds = new List<string>() { systemTaskTypeSend.Id }
            //};
            //var systemTaskJobs = (systemTaskJobService.GetByFilter(systemTaskJobFilter)).OrderBy(s => s.CreatedDateTime).ToList();

            //// Execute jobs
            //while (systemTaskJobs.Any() && !cancellationToken.IsCancellationRequested)
            //{
            //    var systemTaskJob = systemTaskJobs.First();
            //    systemTaskJobs.Remove(systemTaskJob);

            //    // Get email creator
            //    var parameterCreator = systemTaskJob.Parameters.First(p => p.SystemValueTypeId == systemValueTypeCreator.Id);
            //    var creator = serviceProvider.GetRequiredKeyedService<IEmailCreator>(parameterCreator.Value);

            //    try
            //    {
            //        // Send email
            //        await Send(systemTaskJob,
            //                        creator,
            //                        emailService,
            //                        auditEventService, auditEventTypeService,
            //                        systemTaskJobService, systemValueTypeService,
            //                        systemTaskStatusActive,
            //                        systemTaskStatusCompletedSuccess,
            //                        systemTaskStatusCompletedError,
            //                        currentUser,
            //                        cancellationToken);
            //    }
            //    catch (Exception exception)
            //    {
            //        // TODO: Log error
            //    }
            //}
        }

        ///// <summary>
        ///// Sends email
        ///// </summary>
        ///// <param name="systemTaskJob"></param>
        ///// <param name="creator"></param>
        ///// <param name="emailService"></param>
        ///// <param name="passwordResetService"></param>
        ///// <param name="systemValueTypeService"></param>        
        ///// <param name="userService"></param>
        ///// <returns></returns>
        //private async Task Send(SystemTaskJob systemTaskJob,
        //                                    IEmailCreator creator,
        //                                    IEmailService emailService,
        //                                    IAuditEventService auditEventService,
        //                                    IAuditEventTypeService auditEventTypeService,
        //                                    ISystemTaskJobService systemTaskJobService,
        //                                    ISystemValueTypeService systemValueTypeService,
        //                                    SystemTaskStatus systemTaskStatusActive,
        //                                    SystemTaskStatus systemTaskStatusCompletedSuccess,
        //                                    SystemTaskStatus systemTaskStatusCompletedError,
        //                                    User currentUser,
        //                                    CancellationToken cancellationToken)
        //{
        //    // Set job status Active
        //    systemTaskJob.StatusId = systemTaskStatusActive.Id;
        //    systemTaskJobService.Update(systemTaskJob);

        //    // Get system values for parameters
        //    var systemValues = systemTaskJob.Parameters.Select(p => p.ToSystemValue()).ToList();

        //    // Create email            
        //    var subject = creator.GetSubject(systemValues);
        //    var body = creator.GetBody(systemValues);
        //    var emailRecipients = creator.GetRecipientEmails(systemValues);
        //    var ccEmails = new List<string>();

        //    // Send email
        //    if (emailRecipients.Any())
        //    {
        //        await emailService.SendAsync(emailRecipients, ccEmails, body, subject);

        //        // Add audit event
        //        await AddAuditEvenSentAsync(creator, auditEventService, auditEventTypeService, systemValueTypeService, currentUser);
        //    }

        //    // Set job status Completed Succcess
        //    systemTaskJob.StatusId = systemTaskStatusCompletedSuccess.Id;
        //    systemTaskJobService.Update(systemTaskJob);
        //}

        //private async Task AddAuditEvenSentAsync(IEmailCreator creator,
        //                            IAuditEventService auditEventService,
        //                            IAuditEventTypeService auditEventTypeService,
        //                            ISystemValueTypeService systemValueTypeService,
        //                            User currentUser)
        //{
        //    var auditEventType = auditEventTypeService.GetByName(AuditEventTypeNames.SentEmail);
        //    var systemValueTypeCreator = systemValueTypeService.GetByName(SystemValueTypeNames.EmailCreator);

        //    var auditEvent = new AuditEvent()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        CreatedDateTime = DateTimeOffset.UtcNow,
        //        CreatedUserId = currentUser.Id,
        //        TypeId = auditEventType.Id,
        //        Parameters = new List<AuditEventParameter>()
        //            {
        //                new AuditEventParameter()
        //                {
        //                    Id = Guid.NewGuid().ToString(),
        //                    SystemValueTypeId = systemValueTypeCreator.Id,
        //                    Value = creator.Name
        //                }
        //            }
        //    };
        //    await auditEventService.Add(auditEvent);
        //}

    }
}
