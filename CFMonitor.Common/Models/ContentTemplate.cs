using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Models
{
    public class ContentTemplate
    {        
        public string Id { get; set; } = String.Empty;
     
        public string Name { get; set; } = String.Empty;

        public byte[] Content { get; set; } = new byte[0];
    }
}
