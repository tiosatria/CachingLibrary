
using Newtonsoft.Json;

namespace CachingLibrary
{
    public sealed class RedisCache<T>
    {

        public RedisCache(string key)
        {
            this.Key=key;
        }

        public RedisCache(string key, T Data)
        {
            this.Key = key;
            this.Data = Data;
        }

        public string Key { get; set; }
        public T? Data { get; set; }

        public bool HaveData { get; private set; } = false;

        public async Task<bool> GetAsync()
        {
            try
            {
                var strData = (string?)await Redis.db.StringGetAsync(Key);
                if (strData is null) return false;
                Data = JsonConvert.DeserializeObject<T>(strData);
                HaveData = true;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> SaveAsync(TimeSpan? ts = null)
        {
            if(Data is null) return false;
            var strData = JsonConvert.SerializeObject(Data);
            return await Redis.db.StringSetAsync(Key, strData, ts);
        }

        public async Task<bool> DeleteAsync() => await Redis.db.KeyDeleteAsync(Key);

        public static async Task<bool> DeleteAsync(string key)
        {
            return await Redis.db.KeyDeleteAsync(key);
        }

        /// <summary>
        /// Warning! This will erase all redis cache on server. Be very careful when to call this.
        /// </summary>
        /// <returns></returns>
        public static bool Eradicate()
        {
            try
            {
                Redis.server.FlushDatabase();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

    }
}
