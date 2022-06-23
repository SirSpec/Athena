using Website.Options;
using Website.Policies;
using Website.Repositories;
using Website.Services;
using Website.Services.Interpreters;
using Website.Services.Mappers;
using Website.Services.Validators;

namespace Microsoft.Extensions.DependencyInjection;

public static class CompositionRoot
{
    public static IServiceCollection AddDependencies(this IServiceCollection services) =>
        services
            .AddScoped<IPostInterpreter, PostInterpreter>()
            .AddScoped<IPostValidator, PostValidator>()
            .AddScoped<IPostMapper, PostMapper>()
            .AddScoped<IPostService, PostService>();

    public static IServiceCollection AddHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var apiOptions = configuration
                    .GetSection(nameof(ApiOptions))
                    .Get<ApiOptions>();

        services
            .AddHttpClient<IPostRepository, PostRepository>(configureClient =>
            {
                configureClient.DefaultRequestHeaders.Add("User-Agent", "request");
                configureClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                configureClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiOptions.AccessToken}");
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(apiOptions.HttpMessageHandlerLifeTime))
            .AddPolicyHandler(PostApiPolicyFactory.GetRetryPolicy(services))
            .AddPolicyHandler(PostApiPolicyFactory.GetCircuitBreakerPolicy(services));

        return services;
    }
}