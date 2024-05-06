using Refit;

namespace HackerNewsApi;

public interface IHackerNewsApi
{
    [Get("/v0/beststories.json")]
    Task<IEnumerable<long>> GetBestStories(CancellationToken cancellationToken);

    [Get("/v0/item/{id}.json")]
    Task<Story> GetStory(long id, CancellationToken cancellationToken);
}
