namespace Website.Options;

public class AppConfigurationOptions
{
    public string Endpoint { get; set; } = string.Empty;
    public int CacheExpiration { get; set; }
}