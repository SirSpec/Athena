using System.Text.Json.Serialization;

namespace Website.Services;

public class Links
{
    [JsonPropertyName("self")]
    public string? Self { get; set; }

    [JsonPropertyName("git")]
    public string? Git { get; set; }

    [JsonPropertyName("html")]
    public string? Html { get; set; }
}
