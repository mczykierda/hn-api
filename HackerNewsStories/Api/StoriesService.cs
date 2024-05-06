namespace HackerNewsStories.Api;

class StoriesService : IStoriesService
{
    Stories? _storiesOrderedByScoreDesc;

    public bool AreStoriesAvailable => _storiesOrderedByScoreDesc != null;

    public Stories GetBestStoriesOrderedByScoreDesc(int howManyStories) => _storiesOrderedByScoreDesc?.Filter(x => x.Take(howManyStories)) 
                                                                                    ?? throw new StoriesNotYetAvailableException();

    public void SetBestStories(Stories stories) => _storiesOrderedByScoreDesc = stories.Filter(x => x.OrderByDescending(x => x.Score));
}