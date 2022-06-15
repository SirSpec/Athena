using Microsoft.AspNetCore.Html;

namespace Website.Models;

public class PostTeaserViewModel
{
    public string Id { get; init; } = string.Empty;
    public HtmlString PublishingDate { get; init; } = HtmlString.Empty;
    public HtmlString Title { get; init; } = HtmlString.Empty;
    public HtmlString Description { get; init; } = HtmlString.Empty;
    public HtmlString Url { get; init; } = HtmlString.Empty;
}
