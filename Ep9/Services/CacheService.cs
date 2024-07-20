using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Ep9.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task SetAsync<T>(string key, T value)
        {
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });
        }
        public async Task<T?> GetAsync<T>(string key)
        {
            var cachedJson = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(cachedJson))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(cachedJson);
        }
        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
