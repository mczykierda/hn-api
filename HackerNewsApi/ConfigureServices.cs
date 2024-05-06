using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Refit;

namespace HackerNewsApi;

public static class ConfigureServices
{
    public static void AddHackerNewsApi(this IServiceCollection services)
    {
        var retryPolicy = HttpPolicyExtensions
                            .HandleTransientHttpError()
                            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var circuitBreakerPolicy = HttpPolicyExtensions
                                    .HandleTransientHttpError()
                                    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(10));

        services.AddRefitClient<IHackerNewsApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://hacker-news.firebaseio.com"))
                .AddPolicyHandler(retryPolicy)
                .AddPolicyHandler(circuitBreakerPolicy);
    }
}
