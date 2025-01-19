using System.Collections.Concurrent;
using HackerNews.Api.Models;

namespace HackerNews.Api.Services;

public class StoriesThreadSafeCache
{
    private readonly ConcurrentDictionary<long, CacheEntry> _cache = new();
    private readonly TimeSpan _lifetime;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public StoriesThreadSafeCache(TimeSpan lifetime)
    {
        _lifetime = lifetime;
    }
    
    public async Task<Story> GetOrAddAsync(long key, Func<Task<Story>> valueProvider)
    {
        if (_cache.TryGetValue(key, out var cachedEntry) && !cachedEntry.IsExpired)
        {
            return cachedEntry.Value;
        }

        await _semaphore.WaitAsync();
        try
        {
            var value = await valueProvider();
            var expiryTime = DateTime.UtcNow.Add(_lifetime);
            _cache[key] = new CacheEntry(value, expiryTime);
            return value;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private record CacheEntry(Story Value, DateTime ExpiryTime)
    {
        public bool IsExpired => DateTime.UtcNow >= ExpiryTime;
    }
}