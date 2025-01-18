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
    public IAsyncEnumerable<Story> Get(int? numberOfElements = null, CancellationToken cancellationToken = default)
    {
        return _bestStoryRetriever.Get(numberOfElements, cancellationToken);
    }
}