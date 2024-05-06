using HackerNewsApi;
using HackerNewsStories.Api;

namespace HackerNewsStories.StoriesLoading;

class StoriesLoaderFromHackerNewsApi(IHackerNewsApi api, IStoriesService storiesService, ILogger<StoriesLoaderFromHackerNewsApi> logger) : IStoriesLoader
{
    private readonly IHackerNewsApi _api = api;
    private readonly IStoriesService _storiesService = storiesService;
    private readonly ILogger<StoriesLoaderFromHackerNewsApi> _logger = logger;

    public async Task LoadStoriesFromApiAndAddToService(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogDebug("Starting load");

            var stories = new List<Api.Story>();

            var storyIds = await _api.GetBestStories(stoppingToken);
            foreach (var storyId in storyIds)
            {
                if(stoppingToken.IsCancellationRequested)
                {
                    _logger.LogDebug("Loading data cancelled");
                    return;
                }

                try
                {
                    var story = await _api.GetStory(storyId, stoppingToken);
                    stories.Add(Api.Story.From(story));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to get story with ID {storyId}", storyId);
                }
            }

            _storiesService.SetBestStories(new Stories(stories, DateTime.UtcNow));

            _logger.LogDebug("Finished load");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Unable to load stories and add them to service");
        }
    }
}
