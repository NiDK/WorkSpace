using System;
using PwC.C4.Infrastructure.Config;
using JsonHelper = PwC.C4.Infrastructure.Helper.JsonHelper;

namespace PwC.C4.Infrastructure.Cache
{
    public static class Preference
    {
        private static readonly C4CacheServiceClient Client = null;
        private static string appCode = null;

        static Preference()
        {
            if (Client == null)
            {
                Client = new C4CacheServiceClient();
                appCode = AppSettings.Instance.GetAppCode();
            }
        }

        public static string Get(string key)
        {
            var cacheValue = CacheHelper.GetCacheItem<string>(key);
            if (cacheValue != null) return cacheValue;
            cacheValue = GetFrom(appCode, key);
            if (cacheValue != null)
                Set(key, cacheValue);
            return cacheValue;
        }

        public static T Get<T>(string key)
        {
            try
            {
                var cacheValue = CacheHelper.GetCacheItem<T>(key);
                if (cacheValue != null) return cacheValue;
                var json = GetFrom(appCode, key);
                var data = JsonHelper.Deserialize<T>(json);
                if (data != null)
                    Set(key, data);
                return data;
            }
            catch (Exception)
            {
                return default(T);
            }



        }

        public static void Set(string key, string value, TimeSpan? slidingExpiration = null,
            DateTime? absoluteExpiration = null)
        {
            CacheHelper.SetCacheItem(key, value, slidingExpiration, absoluteExpiration);
            SetTo(appCode, key, value);
        }


        public static void Set<T>(string key, T value, TimeSpan? slidingExpiration = null,
            DateTime? absoluteExpiration = null)
        {
            CacheHelper.SetCacheItem(key, value, slidingExpiration, absoluteExpiration);
            var json = JsonHelper.Serialize(value);
            if (json != "[]")
                SetTo(appCode, key, json);
        }

        public static void Remove(string key)
        {
            CacheHelper.RemoveCacheItem(key);
            Client.DeleteKey(appCode, key);
        }

        public static void SetTo(string appcode, string key, string json)
        {
            var enable = AppSettings.Instance.CachePersistenceMode();
            if (enable == "ON")
                Client.Set(appCode, key, json);
        }

        public static string GetFrom(string appcode, string key)
        {
            var enable = AppSettings.Instance.CachePersistenceMode();
            return enable != "OFF" ? Client.Get(appCode, key) : string.Empty;
        }
    }
}
