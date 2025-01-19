using Microsoft.AspNetCore.Mvc.Testing;

namespace PerformanceTests;

public class PerformanceTestAppFactory : WebApplicationFactory<HackerNews.Api.Program>
{
    public HttpClient CreateTestClient() => CreateClient();
}