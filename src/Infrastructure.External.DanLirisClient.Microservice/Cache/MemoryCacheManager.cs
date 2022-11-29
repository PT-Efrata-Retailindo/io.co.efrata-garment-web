
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice.Cache
{
    public class MemoryCacheManager : IMemoryCacheManager
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key, Func<ICacheEntry, T> initial)
        {
            return _cache.GetOrCreate<T>(key, initial);
        }

        public void Set<T>(string key, T data)
        {
            _cache.Set<T>(key, data);
        }
    }

    public interface IMemoryCacheManager
    {
        T Get<T>(string key, Func<ICacheEntry, T> initial);
        void Set<T>(string key, T data);
    }
}
