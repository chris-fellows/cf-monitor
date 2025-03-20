using CFMonitor.Constants;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Utilities
{
    public static class UIUtilities
    {
        public static List<NameAndValue> GetUserRoles()
        {
            return new List<NameAndValue>()
            {
                new NameAndValue()
                {
                    Name = UserRoleNames.Administrator,
                    Value = UserRoleNames.Administrator
                },
                new NameAndValue()
                {
                    Name = UserRoleNames.User,
                    Value = UserRoleNames.User
                },
            };
        }
    }
}
