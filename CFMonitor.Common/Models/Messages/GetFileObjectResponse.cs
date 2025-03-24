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
        public byte[] Content { get; set; } = new byte[0];

        public string Name { get; set; } = String.Empty;

        public GetFileObjectResponse()
        {
            Id = Guid.NewGuid().ToString();
            TypeId = MessageTypeIds.GetFileObjectResponse;
        }
    }
}
