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
                    Name = "Query1.sql",
                    Content = File.ReadAllBytes("D:\\Temp\\Queries\\Query1.sql")
                }
            };

            return list;
        }
    }
}
