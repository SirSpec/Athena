using System.Text.Json.Serialization;

namespace Athena.Website.Repositories.Models;

public class ContentItem
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}