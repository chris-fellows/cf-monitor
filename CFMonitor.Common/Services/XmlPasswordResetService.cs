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
    public class XmlPasswordResetService : XmlEntityWithIdService<PasswordReset, string>, IPasswordResetService
    {
        public XmlPasswordResetService(string folder) : base(folder,
                                                "PasswordReset.*.xml",
                                              (passwordReset) => $"PasswordReset.{passwordReset.Id}.xml",
                                            (passwordResetId) => $"PasswordReset.{passwordResetId}.xml")
        {

        }

        public PasswordReset? GetByUserId(string userId)
        {
            return GetAll().FirstOrDefault(pr => pr.UserId == userId); 
        }
    }
}
