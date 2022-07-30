using Azure.Identity;
using Athena.Website.Extensions;
using Athena.Website.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddAppConfiguration(this IConfigurationBuilder configurationBuilder)
    {
        var appConfigurationOptions = configurationBuilder
            .Build()
            .GetSection(nameof(AppConfigurationOptions))
            .Get<AppConfigurationOptions>();

        var endpointUri = appConfigurationOptions.Endpoint.ToAbsoluteUri();

        return configurationBuilder.AddAzureAppConfiguration(options =>
        {
            options
                .Connect(endpointUri, new ManagedIdentityCredential())
                .ConfigureRefresh(configure => configure
                    .Register("CacheOptions:Sentinel", refreshAll: true)
                    .SetCacheExpiration(TimeSpan.FromMinutes(appConfigurationOptions.CacheTimeToLiveInMinutes)));
        });
    }
}