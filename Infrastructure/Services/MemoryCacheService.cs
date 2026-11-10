using Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services
{
    public sealed class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T? GetCachedResource<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public void RemoveCacheResource(string key)
        {
            _memoryCache.Remove(key);
        }

        public void Set<T>(string key, T cacheResource, TimeSpan expiration)
        {
            _memoryCache.Set(key, cacheResource, expiration);
        }
    }
}
