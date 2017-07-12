using System;
using System.Runtime.Caching;

namespace PwC.C4.Infrastructure.Cache
{
    public static class CacheHelper
    {
        private static readonly Object Locker = new object();

        public static bool SetCacheItem<T>(String key, T cachePopulate, TimeSpan? slidingExpiration = null,
            DateTime? absoluteExpiration = null)
        {
            try
            {
                if (cachePopulate == null) throw new ArgumentNullException("cachePopulate");
                lock (Locker)
                {
                    var item = new CacheItem(key, cachePopulate);
                    var policy = CreatePolicy(slidingExpiration, absoluteExpiration);
                    MemoryCache.Default.Set(item, policy);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static T GetCacheItem<T>(String key)
        {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("Invalid cache key");
            return (T)MemoryCache.Default[key];
        }

        public static void RemoveCacheItem(String key)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("Invalid cache key");
                MemoryCache.Default.Remove(key);
            }
            catch (Exception)
            {
   
            }
            
        }

        public static void Dispose()
        {
            MemoryCache.Default.Dispose();
        }

        private static CacheItemPolicy CreatePolicy(TimeSpan? slidingExpiration, DateTime? absoluteExpiration)
        {
            var policy = new CacheItemPolicy();

            if (absoluteExpiration.HasValue)
            {
                policy.AbsoluteExpiration = absoluteExpiration.Value;
            }
            else if (slidingExpiration.HasValue)
            {
                policy.SlidingExpiration = slidingExpiration.Value;
            }

            policy.Priority = CacheItemPriority.Default;

            return policy;
        }
    }
}
