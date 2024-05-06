# hn-api

The web API getting data from Hacker News API's best stories list.

## How to run the example

### Visual Studio

* Clone the repository
* Open the [HackerNewsStories.sln](HackerNewsStories.sln) file
* Set `HackerNewsStories` project as startup
* Run `https` launch profile. That will open Swagger docs at [this address](https://localhost:7135/swagger/index.html)
* There is also a file [HackerNewsStories.http](/HackerNewsStories/HackerNewsStories.http) which may be used to launch HTTP requests to the api.

### From command line

* Clone the repository
* Go to the created folder and run `dotnet restore`, then `dotnet build`
* then run `dotnet run --project HackerNewsStories`
* open the Swagger docs at [this address](https://localhost:7135/swagger/index.html)

### Notes
* Initially endpoint `/api/best-story` returns HTTP status 503 and header `Retry-After` with value 30 (seconds).  This is because the api is loading stories from HackerNews into memory. Once the data is loaded into memory, the data is provided and HTTP 200 is returned.
* The 30 seconds is the average time I had to download 200 stories from HackerNews (simple for-each loop)
* The 503 status is actually returned by a custom middleware which tests if the data was set by the loader
* There is a background service which refreshes data in memory, it does it every 15 minutes (configurable in appsettings.json - see `RefreshStoriesTimeSpan` key)

### Support for large number of clients
* data is fetched from memory (data refreshed every 15 minutes)
* http response caching (the public cache with age of 30 seconds)

## Assumptions

* Clients of the API can live with stale data 
  * The data from HackerNews API is refreshed every 15 minutes.
  * Also there is a http response caching set up - the max age is 30 seconds
  * I didn't focus on getting data from Hacker News as quickly as possible. 
* I allowed myself to pass the ID of the story to the client, which is on top of the schema you suggested. Reason: the Story has all the characteristics of entity hence it should be identifiable: from client's perspective that would make life way easier.
* when loading data - we just swap one single pointer from old to new data, I didn't use any forms of threads synchronization there. If it were few pointers to change - I'd go for light, spinning ReaderWriterLockSlim.
* no unit tests

## Enhancements to do
* parallelize fetch of story data from HackerNews APi - we have 200 stories to fetch separsately, currently in a for-each loop.
* every time the new set of data is set in StoriesService - save it, and read it at startup
* what is being returned is not ideal - I can easily imagine the requirements for downloading next chunk of items, or paging etc - apart from items it would be good to return also some more information, like e.g. total nmber of stories, or the link to next/previous chunk, also client may want to know how stale the data is (as a whole collection and possibly also per story?). There is a preparation for this: class `Stories`, where some additional properties can be added.
* Learning for myself for the nearest future: 
  * experiment with record structs once I know them ?
  * have the code as minimal API ?