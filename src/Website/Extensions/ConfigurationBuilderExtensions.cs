using Azure.Identity;
using Website.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddAppConfiguration(this IConfigurationBuilder configurationBuilder)
    {
        var appConfigurationOptions = configurationBuilder
            .Build()
            .GetSection(nameof(AppConfigurationOptions))
            .Get<AppConfigurationOptions>();

        return Uri.TryCreate(appConfigurationOptions.Endpoint, UriKind.Absolute, out var endpoint)
            ? configurationBuilder.AddAzureAppConfiguration(options =>
            {
                options
                    .Connect(endpoint, new ManagedIdentityCredential())
                    .ConfigureRefresh(
                        configure => configure
                            .Register("CacheOptions:Sentinel", refreshAll: true)
                            .SetCacheExpiration(TimeSpan.FromHours(appConfigurationOptions.CacheExpiration)));
            })
            : throw new InvalidOperationException("Invalid App Configuration options. ");
    }
}