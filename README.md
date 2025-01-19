# HackerNewsApi

This is .NET 9 Rest API to aggregate all best stories into one endpoint.
In my application I focused mainly on performance. 
I've created a thread safe cache to limit API calls.

To run App:

```shell
cd .\HackerNews.Api
```
```shell
dotnet build
```
```shell
dotnet run
```

Performance tests run from console should not give valuable info about tests. 
To run tests reliably use Visual Studio or Rider. 

In `appsettings.json` is set cache lifetime:
`"CacheLifetimeSeconds" : 60`

Try to manipulate this value to check what happens with API responsibility.