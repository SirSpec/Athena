namespace Website.Options;

public class ApiOptions
{
    public string PostApiUrl { get; set; } = string.Empty;
    public string RawDataApiUrl { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public int HttpMessageHandlerLifeTime { get; set; }
    public int RetryCount { get; set; }
    public int BaseRetryDelay { get; set; }
    public int HandledEventsAllowedBeforeBreaking { get; set; }
    public int DurationOfBreak { get; set; }
}