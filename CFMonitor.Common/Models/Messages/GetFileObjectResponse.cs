using CFMonitor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Models.Messages
{
    public class GetFileObjectResponse : MessageBase
    {
        public FileObject? FileObject { get; set; }

        public GetFileObjectResponse()
        {
            Id = Guid.NewGuid().ToString();
            TypeId = MessageTypeIds.GetFileObjectResponse;
        }
    }
}
