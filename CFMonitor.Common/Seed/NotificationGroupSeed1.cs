using CFMonitor.Constants;
using CFMonitor.Email;
using CFMonitor.EntityReader;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Seed
{
    public class NotificationGroupSeed1 :IEntityReader<NotificationGroup>
    {
        public IEnumerable<NotificationGroup> Read()
        {
            var list = new List<NotificationGroup>()
            {
                new NotificationGroup()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = NotificationGroupNames.GenericAuditEvent,
                    EmailNotificationConfigs = new List<EmailNotificationConfig>()
                    {
                        new EmailNotificationConfig()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Creator = GenericAuditEventEmailCreator.CreatorName,
                            RecipientEmails = ""
                        }
                    }
                },            
                new NotificationGroup()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = NotificationGroupNames.NewUser,
                    EmailNotificationConfigs = new List<EmailNotificationConfig>()
                    {
                        new EmailNotificationConfig()
                        {
                            Id= Guid.NewGuid().ToString(),
                            Creator = NewUserEmailCreator.CreatorName,
                            RecipientEmails = ""
                        }
                    }
                },
                new NotificationGroup()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = NotificationGroupNames.ResetPassword,
                    EmailNotificationConfigs = new List<EmailNotificationConfig>()
                    {
                        new EmailNotificationConfig()
                        {
                            Id= Guid.NewGuid().ToString(),
                            Creator = ResetPasswordEmailCreator.CreatorName,
                            RecipientEmails = ""
                        }
                    }
                }
            };

            return list;
        }
    }
}
