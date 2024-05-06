using System.Text.Json.Serialization;

namespace HackerNewsApi;

[method: JsonConstructor]
public class Story(long id, string title, string url, string by, long time, int score, int descendants)
{
    public long Id { get; } = id;
    public string Title { get; } = title;
    public string Url { get; } = url;
    public string By { get; } = by;
    public long Time { get; } = time;
    public int Score { get; } = score;
    public int Descendants { get; } = descendants;

    [JsonIgnore]
    public DateTime DateTime => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Time);
}
