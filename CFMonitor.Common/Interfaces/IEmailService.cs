using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(List<string> recipientEmails, List<string> ccEmails, string body, string subject);
    }
}
