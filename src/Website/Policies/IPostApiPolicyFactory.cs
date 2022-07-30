using Polly;

namespace Athena.Website.Policies;

public interface IPostApiPolicyFactory
{
    IAsyncPolicy<HttpResponseMessage> GetRetryPolicy();
    IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy();
}