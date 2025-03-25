using CFMonitor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Services
{
    public class MemoryCache : ICache
    {
        private class CacheItem
        {
            public string Key { get; set; } = String.Empty;

            public object? Item { get; set; }

            public DateTimeOffset ExpiryTime { get; set; }
        }

        private Dictionary<string, CacheItem> _items = new();

        public void Clear()
        {
            _items.Clear();
        }

        public void Add<T>(T item, string key, TimeSpan expiry)
        {
            RemoveExpired();

            Remove(key);

            _items.Add(key, new CacheItem() { Key = key, Item = item, ExpiryTime = DateTimeOffset.UtcNow.Add(expiry) });
        }

        public T? Get<T>(string key)
        {
            RemoveExpired();

            if (_items.ContainsKey(key))
            {
                return (T?)_items[key].Item;
            }

            return default(T);
        }

        public void Remove(string key)
        {
            if (_items.ContainsKey(key))
            {
                _items.Remove(key);
            }
        }

        private void RemoveExpired()
        {
            var keys = _items.Keys.Where(key => _items[key].ExpiryTime <= DateTimeOffset.UtcNow).ToList();
            foreach (var key in keys)
            {
                _items.Remove(key);
            }
        }
    }
}
