using System.Text.Json;

namespace HackerNewsStories.Api;

public class StoriesNotYetAvailableMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext httpContext, IStoriesService storiesService)
    {
        if(!storiesService.AreStoriesAvailable)
        {
            var delay = 30;
            httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.Headers.Append("Retry-After", $"{delay}");
            var customResponse = new
            {
                Code = httpContext.Response.StatusCode,
                Message = $"Service is starting up. Please wait {delay} seconds",
            };

            var responseJson = JsonSerializer.Serialize(customResponse);

            await httpContext.Response.WriteAsync(responseJson);
        }
        else
        {
            await _next(httpContext);
        }
    }
}