﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Models
{
    public class NotificationGroup
    {        
        public string Id { get; set; } = String.Empty;
        
        public string Name { get; set; } = String.Empty;

        public List<EmailNotificationConfig> EmailNotificationConfigs { get; set; }
    }
}
