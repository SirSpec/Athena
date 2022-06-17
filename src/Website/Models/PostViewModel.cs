using Microsoft.AspNetCore.Html;

namespace Website.Models;

public class PostViewModel
{
    public static readonly PostViewModel Empty = new PostViewModel();
    public HtmlString PublishingDate { get; init; } = HtmlString.Empty;
    public HtmlString Title { get; init; } = HtmlString.Empty;
    public HtmlString Body { get; init; } = HtmlString.Empty;
}