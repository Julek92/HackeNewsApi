# HackerNewsApi

## Introduction

This is the exercise for recruitment process. The objectives of the exercise are at the bottom of the page.

The app is a .NET 9 REST API that exposes all top stories to the user in a single endpoint.
In my application I focused mainly on performance. 
I've created a thread safe cache to limit API calls.

## Run

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

## The objectives of the exercise

Using ASP.NET Core, implement a RESTful API to retrieve the details of the best n stories from the Hacker News API, as determined by their score, where n isspecified by the caller to the API.

The Hacker News API is documented here: https://github.com/HackerNews/API .

The IDs for the stories can be retrieved from this URI: https://hacker-news.firebaseio.com/v0/beststories.json .

The details for an individual story ID can be retrieved from this URI: https://hacker-news.firebaseio.com/v0/item/21233041.json (in this case for the story with ID 21233041)

The API should return an array of the best n stories as returned by the Hacker News API in descending order of score, in the form:

```json
[
  {
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "url": "https://github.com/uBlockOrigin/uBlock-issues/issues/745"
    "postedBy": "ismaildonmez",
    "time": "2019-10-12T13:43:01+00:00",
    "score": 1716,
    "commentsCount": 572
  },
  {...},
  {...},
  {...},
  {...},
]
```

In addition to the above, your API should be able to efficiently service large numbers of requests without risking overloading of the Hacker News API.

You should share a public repository with us, that should include a README.md file which describes how to run the application, any assumptions you have made, andany enhancements or changes you would make, given the time.
