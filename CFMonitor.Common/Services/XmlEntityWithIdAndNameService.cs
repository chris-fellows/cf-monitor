﻿using CFMonitor.Utilities;
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

        public Task<List<TEntityType>> GetAllAsync()
        {
            var items = new List<TEntityType>();
            foreach (var file in Directory.GetFiles(_folder, _getAllFilePattern))
            {
                items.Add(XmlUtilities.DeserializeFromString<TEntityType>(File.ReadAllText(file)));
            }
            return Task.FromResult(items);
        }

        public Task<TEntityType?> GetByIdAsync(TIdType id)
        {
            var file = Path.Combine(_folder, _getEntityFileNameByIdFunction(id));
            return Task.FromResult(File.Exists(file) ?
                    XmlUtilities.DeserializeFromString<TEntityType>(File.ReadAllText(file)) : default(TEntityType));
        }

        public Task<TEntityType> AddAsync(TEntityType entity)
        {
            var file = Path.Combine(_folder, _getEntityFileNameByEntityFunction(entity));
            File.WriteAllText(file, XmlUtilities.SerializeToString(entity));
            return Task.FromResult(entity);
        }

        public Task<TEntityType> UpdateAsync(TEntityType entity)
        {
            var file = Path.Combine(_folder, _getEntityFileNameByEntityFunction(entity));
            File.WriteAllText(file, XmlUtilities.SerializeToString(entity));
            return Task.FromResult(entity);
        }

        public Task DeleteByIdAsync(TIdType id)
        {
            var file = Path.Combine(_folder, _getEntityFileNameByIdFunction(id));
            if (File.Exists(file)) File.Delete(file);
            return Task.CompletedTask;
        }

        public Task<List<TEntityType>> GetByIdsAsync(List<TIdType> ids)
        {
            var entities = new List<TEntityType>();

            foreach (var id in ids)
            {
                var entity = GetByIdAsync(id).Result;
                if (entity != null) entities.Add(entity);
            }

            return Task.FromResult(entities);
        }

        public async Task<TEntityType?> GetByNameAsync(string name)
        {
            var entity = (await GetAllAsync()).FirstOrDefault(e => _getEntityNameByEntityFunction!(e) == name);
            return entity;
        }
    }
}
