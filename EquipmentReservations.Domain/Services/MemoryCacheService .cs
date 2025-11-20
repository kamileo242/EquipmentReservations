using Microsoft.Extensions.Caching.Memory;

namespace EquipmentReservations.Domain.Services
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public T? Get<T>(string key)
        {
            memoryCache.TryGetValue(key, out T value);
            return value;
        }

        public void Set<T>(string key, T value, TimeSpan duration)
        {
            memoryCache.Set(key, value, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration
            });
        }

        public void Remove(string key)
        {
            memoryCache.Remove(key);
        }
    }
}
