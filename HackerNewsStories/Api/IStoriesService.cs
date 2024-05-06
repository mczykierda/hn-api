namespace HackerNewsStories.Api;

public interface IStoriesService
{
    bool AreStoriesAvailable { get; }
    Stories GetBestStoriesOrderedByScoreDesc(int howManyStories);

    void SetBestStories(Stories stories);
}