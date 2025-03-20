using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Interfaces
{
    public interface IFileSecurityCheckerService
    {
        Task<bool> ValidateCanUploadImageAsync(byte[] content);
    }
}
