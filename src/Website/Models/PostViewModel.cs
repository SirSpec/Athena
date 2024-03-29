using Microsoft.AspNetCore.Html;

namespace Athena.Website.Models;

public class PostViewModel
{
    public HtmlString PublishingDate { get; init; } = HtmlString.Empty;
    public HtmlString Title { get; init; } = HtmlString.Empty;
    public HtmlString Body { get; init; } = HtmlString.Empty;
}