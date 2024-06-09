using CFMonitor.Interfaces;
using CFMonitor.Models.MonitorItems;
using CFUtilities.XML;

namespace CFMonitor.Services
{
    /// <summary>
    /// Service for storing MonitorItem instances in XML format
    /// </summary>
    public class MonitorItemService : XmlItemRepository<MonitorItem, string>, IMonitorItemService
    {
        public MonitorItemService(string folder) : base(folder, (MonitorItem monitorItem) => monitorItem.ID)
        {

        }
    }

    ///// <summary>
    ///// MonitorItem repository using XML serialization to file system
    ///// </summary>
    //public class MonitorItemService : IMonitorItemService
    //{
    //    private string _folder = "";

    //    public MonitorItemService(string folder)
    //    {
    //        _folder = folder;
    //    }

    //    public List<MonitorItem> GetAll()
    //    {
    //        List<MonitorItem> monitorItems = new List<MonitorItem>();
    //        foreach (string file in Directory.GetFiles(_folder, "*.xml"))
    //        {
    //            monitorItems.Add(Get(Path.GetFileNameWithoutExtension(file)));
    //        }
    //        return monitorItems;
    //    }

    //    public void Add(MonitorItem monitorItem)
    //    {
    //        string file = GetMonitorItemPath(monitorItem.ID);
    //        XmlSerialization.SerializeToFile(monitorItem, file);
    //    }

    //    public void Update(MonitorItem monitorItem)
    //    {
    //        string file = GetMonitorItemPath(monitorItem.ID);
    //        XmlSerialization.SerializeToFile(monitorItem, file);   
    //    }

    //    public void Delete(MonitorItem monitorItem)
    //    {
    //        string file = GetMonitorItemPath(monitorItem.ID);
    //        if (File.Exists(file))
    //        {
    //            File.Delete(file);
    //        }            
    //    }

    //    private string GetMonitorItemPath(string id)
    //    {
    //        return Path.Combine(_folder, string.Format("{0}.xml", id));
    //    }

    //    private MonitorItem Get(string id)
    //    {
    //        string file = GetMonitorItemPath(id);

    //        var monitorItem = (MonitorItem)XmlSerialization.DeserializeFromFile(file, typeof(MonitorItem));

    //        /* This errors when deserializing to specific type
    //        MonitorItem monitorItem = null;
    //        List<Type> typeList = Factory.GetMonitorItemTypeList();
    //        foreach (Type type in typeList)
    //        {
    //            if (type.FullName.Contains("URL"))
    //            {
    //                int xxx = 1000;
    //            }
    //            try
    //            {
    //                monitorItem = (MonitorItem)XmlSerialization.DeserializeFromFile(file, type);
    //                break;
    //            }
    //            catch(Exception ex)
    //            {
    //                throw;
    //            }
    //        }
    //        */ 

    //        return monitorItem;
    //    }
    //}
}
