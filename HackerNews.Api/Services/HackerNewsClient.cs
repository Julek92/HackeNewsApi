using System.Runtime.CompilerServices;
using HackerNews.Api.Models;
using Newtonsoft.Json;

namespace HackerNews.Api.Services;

public interface IHackerNewsClient
{
    Task<IEnumerable<int>> GetBestIds(CancellationToken cancellationToken);
    IAsyncEnumerable<Story> GetStories(HashSet<int> ids, CancellationToken cancellationToken);
    Task<Story> GetStory(int id, CancellationToken cancellationToken);
}

public class HackerNewsClient : IHackerNewsClient
{
    private readonly HttpClient _httpClient;
    private const string GetBestStoriesUrl = "beststories.json";
    private string GetStoryUrl(int id) => $"item/{id}.json";

    public HackerNewsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<IEnumerable<int>> GetBestIds(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(GetBestStoriesUrl, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
        }
        
        var jsonString = await response.Content.ReadAsStringAsync(cancellationToken);
        var stories = JsonConvert.DeserializeObject<List<int>>(jsonString);
        if (stories is null)
        {
            throw new Exception("Could not get stories");
        }

        return stories;
    }

    public async IAsyncEnumerable<Story> GetStories(HashSet<int> ids, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var tasks = ids.Select(id => GetStory(id, cancellationToken));

        var stories = await Task.WhenAll(tasks);

        foreach (var story in stories.OrderBy(x => x.Score).Reverse())
        {
            yield return story;
        }
    }

    public async Task<Story> GetStory(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(GetStoryUrl(id), cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
        }
        
        var jsonString = await response.Content.ReadAsStringAsync(cancellationToken);

        return DeserializeStory(jsonString);
    }
    
    private static Story DeserializeStory(string jsonString)
    {
        try
        {
            var deserialized = JsonConvert.DeserializeObject<dynamic>(jsonString);
            string title = deserialized!["title"];
            string url = deserialized["url"];
            string postedBy = deserialized["by"];
            long unixTimestamp = deserialized["time"];
            int score = deserialized["score"];
            int commentsCount = deserialized["descendants"];
        
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;

            return new Story(title, url, postedBy, dateTime, score, commentsCount);
        }
        catch (Exception e)
        {
            throw new Exception("Error in story deserialization", e);
        }
    }
}