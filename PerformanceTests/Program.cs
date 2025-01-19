using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using PerformanceTests;

var factory = new PerformanceTestAppFactory();
var httpClient = factory.CreateTestClient();

var scenario = Scenario.Create("little_traffic", async context =>
    {
        // begin with 1 element, but value is rising in every test
        var numberOfElements = context.InvocationNumber <= 200 ? context.InvocationNumber : 200;

        var request = Http.CreateRequest("GET", $"api/bestStories/{numberOfElements}");
        var response = await Http.Send(httpClient, request);
        return response;
    })
    .WithWarmUpDuration(TimeSpan.FromSeconds(5))
    .WithLoadSimulations(LoadSimulation.NewInject(4, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10)));

NBomberRunner.RegisterScenarios(scenario)
    .Run();