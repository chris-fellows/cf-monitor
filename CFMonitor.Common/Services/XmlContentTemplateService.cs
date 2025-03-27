using CFMonitor.Interfaces;
using CFMonitor.Models;

namespace CFMonitor.Services
{
    public class XmlContentTemplateService : XmlEntityWithIdAndNameService<ContentTemplate, string>, IContentTemplateService
    {
        public XmlContentTemplateService(string folder) : base(folder,
                                                "ContentTemplate.*.xml",
                                                (contentTemplate) => $"ContentTemplate.{contentTemplate.Id}.xml",
                                                (contentTemplateId) => $"ContentTemplate.{contentTemplateId}.xml",
                                                (contentTemplate) => contentTemplate.Name)
        {

        }
    }
}
