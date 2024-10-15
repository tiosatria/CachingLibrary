
namespace CachingLibrary
{
    public class CacheObject<T> : ICache<T>
    {

        public CacheObject(string key, T val)
        {
            Key = key;
            Value = val;
        }

        public CacheObject(string key)
        {
            Key = key;
        }

        protected CacheObject(ICacheProvider provider, string key)
        {
            _provider=provider;
            Key=key;
        }

        protected CacheObject(ICacheProvider provider, string key, T val)
        {
            Key=key;
            Value = val;
            _provider= provider;
        }

        private readonly ICacheProvider? _provider;


        public virtual async Task<T?> GetAsync()
        {
            if (_provider is null)
                throw new InvalidOperationException(
                    "please specify cache provider in constructor when using GetAsync function");
            var res = await _provider.GetAsync<T>(Key);
            Value = res;
            return res;
        }

        public virtual async Task<bool> StoreAsync(TimeSpan ts)
        {
            if (_provider is null)
                throw new InvalidOperationException(
                    "please specify cache provider in constructor when using StoreAsync function");
            if (Value is null) return false;
            return await _provider.StoreAsync(this, ts);
        }

        public virtual async Task<bool> RemoveAsync()
        {
            if (_provider is null)
                throw new InvalidOperationException(
                    "please specify cache provider in constructor when using RemoveAsync function");
            return await _provider.RemoveAsync(Key);
        }

        public string Key { get; set; }
        public T? Value { get; set; }

    }
}
