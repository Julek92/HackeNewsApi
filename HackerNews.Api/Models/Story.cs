namespace HackerNews.Api.Models;

public record Story(string Title, string Url, string PostedBy, DateTime Time, int Score, int CommentsCount);