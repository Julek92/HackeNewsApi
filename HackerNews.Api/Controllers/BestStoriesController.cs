using HackerNews.Api.Models;
using HackerNews.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Api.Controllers;

[ApiController]
[Route("api/bestStories")]
public class BestStoriesController : Controller
{
    private readonly IBestStoryRetriever _bestStoryRetriever;

    public BestStoriesController(IBestStoryRetriever bestStoryRetriever)
    {
        _bestStoryRetriever = bestStoryRetriever;
    }
    [HttpGet("{numberOfElements:int?}")]
    public async Task<IEnumerable<Story>> Get(int? numberOfElements = null, CancellationToken cancellationToken = default)
    {
        var asyncStories = _bestStoryRetriever.Get(numberOfElements, cancellationToken);
        var stories = new List<Story>();
        await foreach (var story in asyncStories)
        {
            stories.Add(story);
        }

        return stories.OrderBy(s => s.Score).Reverse();
    }
}