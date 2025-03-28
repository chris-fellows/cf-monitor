using CFMonitor.Common.Interfaces;
using CFMonitor.Models;

namespace CFMonitor.Services
{
    public class XmlNameValueItemService : XmlEntityWithIdAndNameService<NameValueItem, string>, INameValueItemService
    {
        public XmlNameValueItemService(string folder) : base(folder,
                                                "NameValueItem.*.xml",
                                              (nameValueItem) => $"NameValueItem.{nameValueItem.Id}.xml",
                                                (nameValueItemId) => $"NameValueItem.{nameValueItemId}.xml",
                                                (nameValueItem) => nameValueItem.Name)
        {

        }
    }
}
