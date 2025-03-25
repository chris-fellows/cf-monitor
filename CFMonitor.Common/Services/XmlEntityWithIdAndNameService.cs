using CFMonitor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Services
{
    public class XmlEntityWithIdAndNameService<TEntityType, TIdType>
    {
        protected readonly string _folder;
        protected readonly string _getAllFilePattern;                                       // E.g. "UserSettings.*.xml"
        protected readonly Func<TEntityType, string> _getEntityFileNameByEntityFunction;    // Returns file name from entity
        protected readonly Func<TIdType, string> _getEntityFileNameByIdFunction;            // Returns file name from id
        protected readonly Func<TEntityType, string>? _getEntityNameByEntityFunction;        // 

        public XmlEntityWithIdAndNameService(string folder,
                                    string getAllFilePattern,
                                    Func<TEntityType, string> getEntityFileNameByEntityFunction,
                                    Func<TIdType, string> getEntityFileNameByIdFunction,
                                    Func<TEntityType, string>? getEntityNameByEntityFunction)

        {
            _folder = folder;
            _getAllFilePattern = getAllFilePattern;
            _getEntityFileNameByEntityFunction = getEntityFileNameByEntityFunction;
            _getEntityFileNameByIdFunction = getEntityFileNameByIdFunction;
            _getEntityNameByEntityFunction = getEntityNameByEntityFunction;

            Directory.CreateDirectory(folder);
        }

        public List<TEntityType> GetAll()
        {
            var items = new List<TEntityType>();
            foreach (var file in Directory.GetFiles(_folder, _getAllFilePattern))
            {
                items.Add(XmlUtilities.DeserializeFromString<TEntityType>(File.ReadAllText(file)));
            }
            return items;
        }

        public TEntityType? GetById(TIdType id)
        {
            var file = Path.Combine(_folder, _getEntityFileNameByIdFunction(id));
            return File.Exists(file) ?
                    XmlUtilities.DeserializeFromString<TEntityType>(File.ReadAllText(file)) : default(TEntityType);
        }

        public void Add(TEntityType entity)
        {
            var file = Path.Combine(_folder, _getEntityFileNameByEntityFunction(entity));
            File.WriteAllText(file, XmlUtilities.SerializeToString(entity));
        }

        public void Update(TEntityType entity)
        {
            var file = Path.Combine(_folder, _getEntityFileNameByEntityFunction(entity));
            File.WriteAllText(file, XmlUtilities.SerializeToString(entity));
        }

        public void DeleteById(TIdType id)
        {
            var file = Path.Combine(_folder, _getEntityFileNameByIdFunction(id));
            if (File.Exists(file)) File.Delete(file);
        }

        public List<TEntityType> GetByIds(List<TIdType> ids)
        {
            var entities = new List<TEntityType>();

            foreach (var id in ids)
            {
                var entity = GetById(id);
                if (entity != null) entities.Add(entity);
            }

            return entities;
        }

        public TEntityType? GetByName(string name)
        {
            return GetAll().FirstOrDefault(e => _getEntityNameByEntityFunction!(e) == name);
        }
    }
}
