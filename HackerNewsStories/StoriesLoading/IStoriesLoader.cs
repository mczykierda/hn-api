namespace HackerNewsStories.StoriesLoading;

public interface IStoriesLoader
{
    Task LoadStoriesFromApiAndAddToService(CancellationToken stoppingToken);
}
