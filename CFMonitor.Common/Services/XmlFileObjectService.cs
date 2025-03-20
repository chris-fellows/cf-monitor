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
    public class XmlFileObjectService : XmlEntityWithIdStoreService<FileObject, string>, IFileObjectService
    {
        public XmlFileObjectService(string folder) : base(folder,
                                                "FileObject.*.xml",
                                              (fileObject) => $"FileObject.{fileObject.Id}.xml",
                                                (fileObjectId) => $"FileObject.{fileObjectId}.xml")
        {

        }
    }
}
