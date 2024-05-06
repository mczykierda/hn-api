using Microsoft.AspNetCore.Mvc;

namespace HackerNewsStories.Api
{
    [ApiController]
    [Route("api")]
    public class StoryController(IStoriesService service) : ControllerBase
    {
        private readonly IStoriesService _service = service;

        [HttpGet("best-story")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, VaryByQueryKeys = ["n"])]
        public IEnumerable<Story> GetBestStories([FromQuery(Name = "n")] int howManyStories = 20)
        {
            var stories = _service.GetBestStoriesOrderedByScoreDesc(howManyStories);
            return stories.Items;
        }
    }
}
