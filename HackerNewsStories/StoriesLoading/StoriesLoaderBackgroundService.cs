namespace HackerNewsStories.StoriesLoading;

class StoriesLoaderBackgroundService(IStoriesLoader loader, IConfiguration configuration, ILogger<StoriesLoaderBackgroundService> logger) : BackgroundService
{
    private readonly IStoriesLoader _loader = loader;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<StoriesLoaderBackgroundService> _logger = logger;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var delay = _configuration.GetValue<TimeSpan>("RefreshStoriesTimeSpan");
        if(delay == TimeSpan.Zero)
        {
            delay = TimeSpan.FromMinutes(5);
        }
        _logger.LogDebug($"Stories refresh every {delay}");


        while (!stoppingToken.IsCancellationRequested)
        {
            await _loader.LoadStoriesFromApiAndAddToService(stoppingToken);

            await Task.Delay(delay, stoppingToken);
        }
    }
}
