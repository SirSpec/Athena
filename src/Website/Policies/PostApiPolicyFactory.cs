using System.Net;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Athena.Website.Options;
using Athena.Website.Repositories;

namespace Athena.Website.Policies;

public class PostApiPolicyFactory : IPostApiPolicyFactory
{
    private readonly ILogger<PostRepository> _postRepositoryLogger;
    private readonly ApiOptions _apiOptions;

    public PostApiPolicyFactory(
        ILogger<PostRepository> postRepositoryLogger,
        IOptions<ApiOptions> apiOptions)
    {
        _postRepositoryLogger = postRepositoryLogger;
        _apiOptions = apiOptions.Value;
    }

    public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(message => message.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(_apiOptions.RetryCount, retryAttempt => GetExponentialBackoff(retryAttempt),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                    _postRepositoryLogger.LogError(
                        $"Connecting to API failed. Delaying for {timespan.TotalMilliseconds}ms, retry:{retryAttempt}."));

    public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(message => message.StatusCode == HttpStatusCode.NotFound)
            .OrResult(message => message.StatusCode == HttpStatusCode.TooManyRequests)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: _apiOptions.HandledEventsAllowedBeforeBreaking,
                durationOfBreak: TimeSpan.FromMinutes(_apiOptions.DurationOfBreakInMinutes),
                onBreak: (result, timeSpan, context) =>
                {
                    _postRepositoryLogger.LogError($"Connection to API is onBreak for {timeSpan.TotalMilliseconds}ms.");
                },
                onHalfOpen: () =>
                {
                    _postRepositoryLogger.LogError($"Connection to API is onBreak again.");
                },
                onReset: context =>
                {
                    _postRepositoryLogger.LogInformation($"Connection to API has been reset.");
                });

    private TimeSpan GetExponentialBackoff(int retryAttempt) =>
        TimeSpan.FromSeconds(Math.Pow(_apiOptions.BaseRetryDelayInSeconds, retryAttempt));
}