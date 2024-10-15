using System.Data.Common;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CachingLibrary
{
    public class RedisCacheProvider : ICacheProvider
    {
        public RedisCacheProvider(string connString)
        {
            var conn = ConnectionMultiplexer.Connect(connString);
            _redis = conn.GetDatabase();
            _server = conn.GetServer(connString.Split(',')[0]);
        }

        public RedisCacheProvider(ConnectionMultiplexer multiplexer)
        {
            _redis = multiplexer.GetDatabase();
            _server = multiplexer.GetServer(multiplexer.Configuration.Split(',')[0]);
        }

        private readonly IDatabase _redis;
        private readonly IServer _server;

        public T? Get<T>(string key)
        {
            var strData = (string?) _redis.StringGet(key);
            return strData != null ? JsonConvert.DeserializeObject<T>(strData) : default;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var strData = (string?)await _redis.StringGetAsync(key);
            return strData != null ? JsonConvert.DeserializeObject<T>(strData) : default;
        }

        public bool Remove(string key) => _redis.KeyDelete(key);

        public Task<bool> RemoveAsync(string key) => _redis.KeyDeleteAsync(key);

        public bool Store<T>(ICache<T> cache, TimeSpan? ts = null)
        {
            if (cache.Value is null) return false;
            var dataStr = JsonConvert.SerializeObject(cache.Value);
            return _redis.StringSet(cache.Key, dataStr, ts);
        }

        public async Task<bool> StoreAsync<T>(ICache<T> cache, TimeSpan? ts = null)
        {
            if (cache.Value is null) return false;
            var dataStr = JsonConvert.SerializeObject(cache.Value);
            return await _redis.StringSetAsync(cache.Key, dataStr, ts);
        }

        public bool ClearAll()
        {
            try
            {
                _server.FlushDatabase();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ClearAllAsync()
        {
            try
            {
                await _server.FlushDatabaseAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task<bool> IsExistAsync(string key) => _redis.KeyExistsAsync(key);

        public bool IsExist(string key) => _redis.KeyExists(key);

    }
}
