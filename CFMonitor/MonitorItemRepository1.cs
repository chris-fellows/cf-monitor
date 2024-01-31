using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CFUtilities.XML;

namespace CFMonitor
{
    /// <summary>
    /// MonitorItem repository using XML serialization to file system
    /// </summary>
    public class MonitorItemRepository1 : IMonitorItemRepository
    {
        private string _folder = "";

        public MonitorItemRepository1(string folder)
        {
            _folder = folder;
        }

        public List<MonitorItem> GetAll()
        {
            List<MonitorItem> monitorItems = new List<MonitorItem>();
            foreach (string file in Directory.GetFiles(_folder, "*.xml"))
            {
                monitorItems.Add(Get(Path.GetFileNameWithoutExtension(file)));
            }
            return monitorItems;
        }

        public void Insert(MonitorItem monitorItem)
        {
            string file = GetMonitorItemPath(monitorItem.ID);
            XmlSerialization.SerializeToFile(monitorItem, file);
        }

        public void Update(MonitorItem monitorItem)
        {
            string file = GetMonitorItemPath(monitorItem.ID);
            XmlSerialization.SerializeToFile(monitorItem, file);   
        }

        public void Delete(MonitorItem monitorItem)
        {
            string file = GetMonitorItemPath(monitorItem.ID);
            if (File.Exists(file))
            {
                File.Delete(file);
            }            
        }

        private string GetMonitorItemPath(string id)
        {
            return Path.Combine(_folder, string.Format("{0}.xml", id));
        }

        private MonitorItem Get(string id)
        {
            string file = GetMonitorItemPath(id);
            MonitorItem monitorItem = null;
            List<Type> typeList = Factory.GetMonitorItemTypeList();
            foreach (Type type in typeList)
            {
                try
                {
                    monitorItem = (MonitorItem)XmlSerialization.DeserializeFromFile(file, type);
                    break;
                }
                catch { };
            }
            return monitorItem;
        }
    }
}
