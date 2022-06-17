using System.Text.Json.Serialization;

namespace Website.Repositories.Models;

public class ContentItem
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}