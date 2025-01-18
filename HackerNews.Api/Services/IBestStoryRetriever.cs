using System.Runtime.CompilerServices;
using HackerNews.Api.Models;

namespace HackerNews.Api.Services;

public interface IBestStoryRetriever
{
    IAsyncEnumerable<Story> Get(int? number, CancellationToken cancellationToken);
}

public class BestStoryRetriever : IBestStoryRetriever
{
    private readonly IHackerNewsClient _client;

    public BestStoryRetriever(IHackerNewsClient client)
    {
        _client = client;
    }

    public async IAsyncEnumerable<Story> Get(int? number, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var ids = await _client.GetBestIds(cancellationToken);
        var idsToLoad = number is null ? ids : ids.Take(number.Value);
        var stories = _client.GetStories(idsToLoad.ToHashSet(), cancellationToken);
        await foreach (var story in stories)
        {
            yield return story;
        }
    }
}