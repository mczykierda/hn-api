using System.Text.Json.Serialization;

namespace HackerNewsStories.Api;

public class Story(long id, string title, string uri, string postedBy, DateTime time, int score, int commentCount)
{
    //[JsonIgnore] //uncomment it if you really think the ID is not important for this "entity-like" object
    public long Id { get; } = id;
    public string Title { get; } = title;
    public string Uri { get; } = uri;
    public string PostedBy { get; } = postedBy;
    public DateTime Time { get; } = time;
    public int Score { get; } = score;
    public int CommentCount { get; } = commentCount;

    public static Story From(HackerNewsApi.Story story)
    {
        return new(story.Id, story.Title, story.Url, story.By, story.DateTime, story.Score, story.Descendants);
    }
}
