using System.Net;
using Polly;
using Polly.Extensions.Http;
using Website.Repositories;

namespace Website.Policies;

public class PostApiPolicyFactory
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IServiceCollection services) =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(message => message.StatusCode == HttpStatusCode.NotFound)
            .OrResult(message => message.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                    services
                        .BuildServiceProvider()
                        .GetRequiredService<ILogger<PostRepository>>()
                        .LogError(
                            $"Connecting to API failed. Delaying for {timespan.TotalMilliseconds}ms, retry:{retryAttempt}."));

    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(IServiceCollection services)
    {
        var logger = services.BuildServiceProvider().GetRequiredService<ILogger<PostRepository>>();

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(message => message.StatusCode == HttpStatusCode.TooManyRequests)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromMinutes(1),
                onBreak: (result, timeSpan, context) =>
                {
                    logger.LogError($"Connection to API is onBreak for {timeSpan.TotalMilliseconds}ms.");
                },
                onHalfOpen: () =>
                {
                    logger.LogError($"Connection to API is onBreak again.");
                },
                onReset: context =>
                {
                    logger.LogInformation($"Connection to API has been reset.");
                });
    }
}