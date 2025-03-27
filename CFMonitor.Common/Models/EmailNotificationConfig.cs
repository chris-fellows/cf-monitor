using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Models
{
    public class EmailNotificationConfig
    {        
        public string Id { get; set; } = String.Empty;
     
        public string Creator { get; set; } = String.Empty;

        public string RecipientEmails { get; set; } = String.Empty;
    }
}
