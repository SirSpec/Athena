using Website.Services;
using Website.Services.Interpreters;

namespace Microsoft.Extensions.DependencyInjection;

public static class CompositionRoot
{
    public static IServiceCollection AddDependencies(this IServiceCollection services) =>
        services
            .AddScoped<IPostInterpreter, PostInterpreter>()
            .AddScoped<IPostService, PostService>();
}