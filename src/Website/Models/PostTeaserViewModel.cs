using Microsoft.AspNetCore.Html;

namespace Website.Models;

public class PostTeaserViewModel
{
    public HtmlString Name { get; init; } = HtmlString.Empty;
    public HtmlString PublishingDate { get; init; } = HtmlString.Empty;
    public HtmlString Title { get; init; } = HtmlString.Empty;
    public HtmlString Description { get; init; } = HtmlString.Empty;
}