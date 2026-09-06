namespace Application.Interfaces
{
    public interface ICacheService
    {

        T? GetCachedResource<T>(string key);
        void Set<T>(string key, T cacheResource, TimeSpan expiration);
        void RemoveCacheResource(string key);
        Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T?>> factory, TimeSpan expiration);
    }
}
