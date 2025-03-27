using CFMonitor.EntityReader;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Seed
{
    public class ContentTemplateSeed1 : IEntityReader<ContentTemplate>
    {
        public IEnumerable<ContentTemplate> Read()
        {
            var list = new List<ContentTemplate>()
            {
                new ContentTemplate()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Generic Audit Event Email",
                    Content = Encoding.UTF8.GetBytes("<html>" +
                            "<head>" +
                            "</head>" +
                            "<body>" +
                            "{AuditEventValues}" +      // Table of values
                            "</body>" +
                            "</html>")
                },              
                new ContentTemplate()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "New User Email",
                    Content = Encoding.UTF8.GetBytes("<html>" +
                            "<head>" +
                            "</head>" +
                            "<body>" +
                            "Welcome to CF Issue Tracker. The user {User.Email} has been registered as a new user." +
                            "</body>" +
                            "</html>")
                },
                new ContentTemplate()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Reset Password Email",
                    Content = Encoding.UTF8.GetBytes("<html>" +
                            "<head>" +
                            "</head>" +
                            "<body>" +
                            "A request has been created to reset the password for {User.Email}.<BR/><BR>" +
                            "Please click <a href='{PasswordReset.Url}'>here</a> to enter a new password." +
                            "</body>" +
                            "</html>")
                },
            };

            return list;
        }

    }
}
