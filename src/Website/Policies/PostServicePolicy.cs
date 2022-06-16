using System.Net;
using Polly;
using Polly.Extensions.Http;
using Website.Services;

namespace Website.Policies;

public class PostServicePolicyFactory
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IServiceCollection services) =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(message => message.StatusCode == HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                    services
                        .BuildServiceProvider()
                        .GetRequiredService<ILogger<PostService>>()
                        .LogError(
                            $"Connection to API failed. Delaying for {timespan.TotalMilliseconds}ms, retry:{retryAttempt}."));

    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(IServiceCollection services)
    {
        var logger = services.BuildServiceProvider().GetRequiredService<ILogger<PostService>>();

        return HttpPolicyExtensions
            .HandleTransientHttpError()
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
                    logger.LogInformation($"Connection to API have been reset.");
                });
    }
}