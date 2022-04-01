using Microsoft.Extensions.Caching.Memory;

namespace ScrumEstimationServerApp.Helper
{
    public static class Extensions
    {
        
        public static T GetValue<T>(this IMemoryCache _memoryCache, string key, Func<T> acquire)
        {
            if(!_memoryCache.TryGetValue(key, out T value))
            {
                var cacheValue = (T)acquire();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _memoryCache.Set(key, cacheValue, cacheEntryOptions);
            }
            return value;
        }

        public static void SetValue<T>(this IMemoryCache _memoryCache, string key, T value)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(10));
            _memoryCache.Set(key, value, cacheEntryOptions);
        }
    }
}
