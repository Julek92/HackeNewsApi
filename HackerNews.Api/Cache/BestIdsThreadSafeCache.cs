namespace HackerNews.Api.Cache;

public class BestIdsThreadSafeCache
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private IEnumerable<long>? _cache;
    private DateTime _expiryTime;
    private readonly TimeSpan _lifetime;

    public BestIdsThreadSafeCache(TimeSpan lifetime)
    {
        _lifetime = lifetime;
        _expiryTime = DateTime.MinValue;
    }

    public async Task<IEnumerable<long>> GetOrAdd(Func<Task<IEnumerable<long>>> dataProvider)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (_cache != null && DateTime.Now < _expiryTime)
            {
                return _cache;
            }

            _cache = await dataProvider.Invoke();
            _expiryTime = DateTime.Now.Add(_lifetime);

            return _cache;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}