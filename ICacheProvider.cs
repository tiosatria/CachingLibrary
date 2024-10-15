using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachingLibrary
{
    public interface ICacheProvider
    {
        T? Get<T>(string key);
        Task<T?> GetAsync<T>(string key);
        bool Remove(string key);
        Task<bool> RemoveAsync(string key);
        bool Store<T>(ICache<T> cache, TimeSpan? ts = null); 
        Task<bool> StoreAsync<T>(ICache<T> cache,TimeSpan? ts=null);
        bool ClearAll();
        Task<bool> ClearAllAsync();
        Task<bool> IsExistAsync(string key);
        bool IsExist(string key);
    }
}
