using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace StepOut.Models
{
    public static class CacheProvidor
    {

        private static readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions() { });

        //private static CacheProvidor Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new CacheProvidor();
        //        }
        //        return instance;
        //    }
        //} 

        public static void Set<T>(string key, T value, DateTimeOffset absoluteExpiry)
        {
            _cache.Set(key, value, absoluteExpiry);
        }

        public static T Get<T>(string key)
        {
            if (_cache.TryGetValue(key, out T value))
                return value;
            else
                return default(T);
        }
    }
}
