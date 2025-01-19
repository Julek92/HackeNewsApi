using System.Runtime.CompilerServices;
using HackerNews.Api.Cache;
using HackerNews.Api.Models;

namespace HackerNews.Api.Services;

public interface IBestStoryRetriever
{
    IAsyncEnumerable<Story> Get(int? number, CancellationToken cancellationToken);
}

public class BestStoryRetriever : IBestStoryRetriever
{
    private readonly IHackerNewsClient _client;
    private readonly BestIdsThreadSafeCache _idsCache;
    private readonly StoriesThreadSafeCache _storiesCache;

    public BestStoryRetriever(IHackerNewsClient client, BestIdsThreadSafeCache idsCache,
        StoriesThreadSafeCache storiesCache)
    {
        _client = client;
        _idsCache = idsCache;
        _storiesCache = storiesCache;
    }

    public async IAsyncEnumerable<Story> Get(int? number, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var allBestIds = await _idsCache.GetOrAdd(() => _client.GetBestIds(cancellationToken));
        var ids = number is null || number > allBestIds.Count() ? allBestIds : allBestIds.Take(number.Value);
        var idsToLoad = ids.ToHashSet();

        var tasks = idsToLoad.Select(id => _storiesCache.GetOrAddAsync(id, () => _client.GetStory(id, cancellationToken)));
        var stories = Task.WhenEach(tasks);
        await foreach (var story in stories)
        {
            yield return await story;
        }
    }
}