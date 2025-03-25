using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Interfaces
{
    public interface ICache
    {
        void Clear();

        void Add<T>(T item, string key, TimeSpan expiry);

        T? Get<T>(string key);

        void Remove(string key);
    }
}
