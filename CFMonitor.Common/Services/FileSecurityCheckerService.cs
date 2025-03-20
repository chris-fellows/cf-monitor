using CFMonitor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Services
{
    public class FileSecurityCheckerService : IFileSecurityCheckerService
    {
        public Task<bool> ValidateCanUploadImageAsync(byte[] content)
        {
            return Task.FromResult(true);
        }
    }
}
