using CFMonitor.EntityReader;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Seed
{
    public class FileObjectSeed1 : IEntityReader<FileObject>
    {
        public IEnumerable<FileObject> Read()
        {
            var list = new List<FileObject>()
            {
                new FileObject()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Errors.sql",
                    Content = File.ReadAllBytes("D:\\Data\\Dev\\C#\\cf-monitor-local\\FileObjects\\SQL\\Errors.sql")
                },
                new FileObject()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Check MS Office installed.ps1",
                    Content = File.ReadAllBytes("D:\\Data\\Dev\\C#\\cf-monitor-local\\FileObjects\\PowerShell\\Check MS Office installed.ps1")
                }
            };

            return list;
        }
    }
}
