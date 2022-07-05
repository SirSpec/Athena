using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Website.Options;
using Website.Policies;
using Website.Repositories;
using Website.Services;
using Website.Services.Mappers;

namespace Microsoft.Extensions.DependencyInjection;

public static class CompositionRoot
{
    public static IServiceCollection AddDependencies(this IServiceCollection services) =>
        services
            .AddScoped<IPostMapper, PostMapper>()
            .AddScoped<PostService>()
            .AddScoped<IPostService, CachedPostService>(provider => new CachedPostService(
                provider.GetRequiredService<IOptionsSnapshot<CacheOptions>>(),
                provider.GetRequiredService<IMemoryCache>(),
                provider.GetRequiredService<PostService>()
            ))
            .AddScoped<IPostApiPolicyFactory, PostApiPolicyFactory>();

    public static IServiceCollection AddHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var apiOptions = configuration
            .GetSection(nameof(ApiOptions))
            .Get<ApiOptions>();

        var policyFactory = services
            .BuildServiceProvider()
            .GetRequiredService<IPostApiPolicyFactory>();

        services
            .AddHttpClient<IPostRepository, PostRepository>(configureClient =>
            {
                var accessToken = string.IsNullOrWhiteSpace(apiOptions.AccessToken) is false
                    ? apiOptions.AccessToken
                    : throw new InvalidOperationException(
                        "Missing Access Token. Please specify it under ApiOptions:AccessToken config section.");

                configureClient.DefaultRequestHeaders.Add("User-Agent", "request");
                configureClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                configureClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(apiOptions.HttpMessageHandlerLifeTimeInMinutes))
            .AddPolicyHandler(policyFactory.GetRetryPolicy())
            .AddPolicyHandler(policyFactory.GetCircuitBreakerPolicy());

        return services;
    }
}