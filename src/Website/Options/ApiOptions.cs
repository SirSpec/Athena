namespace Website.Options;

public class ApiOptions
{
    public string PostApiUrl { get; set; } = string.Empty;
    public string RawDataApiUrl { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public int HttpMessageHandlerLifeTimeInMinutes { get; set; }
    public int RetryCount { get; set; }
    public int BaseRetryDelayInSeconds { get; set; }
    public int HandledEventsAllowedBeforeBreaking { get; set; }
    public int DurationOfBreakInMinutes { get; set; }
}