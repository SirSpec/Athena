using Polly;

namespace Website.Policies;

public interface IPostApiPolicyFactory
{
    IAsyncPolicy<HttpResponseMessage> GetRetryPolicy();
    IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy();
}