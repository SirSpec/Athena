using Website.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddAppConfiguration(this IConfigurationBuilder configurationBuilder)
    {
        var configuration = configurationBuilder.Build();
        var connectionString = configuration.GetConnectionString("AppConfigurationService");

        return string.IsNullOrWhiteSpace(connectionString) is false
            ? configurationBuilder.AddAzureAppConfiguration(options =>
            {
                var cacheExpiration = configuration.GetValue<int>("AppConfigurationOptions:CacheExpiration");
                options.Connect(connectionString).ConfigureRefresh(
                    configure => configure
                        .Register(nameof(CacheOptions))
                        .SetCacheExpiration(TimeSpan.FromHours(cacheExpiration))
                );
            })
            : configurationBuilder;
    }
}