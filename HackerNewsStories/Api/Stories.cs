namespace HackerNewsStories.Api;

public class Stories(IEnumerable<Story> items, DateTime cachedAt)
{
    public IEnumerable<Story> Items { get; } = items;
    public DateTime CachedAt { get; } = cachedAt;

    public Stories Filter(Func<IEnumerable<Story>, IEnumerable<Story>> transformation) => new(transformation(Items).ToArray(), CachedAt);
}