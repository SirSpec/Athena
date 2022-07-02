using Microsoft.AspNetCore.Html;

namespace Website.Extensions;

public static class StringExtensions
{
    public static HtmlString ToHtmlString(this string @string) =>
        new HtmlString(@string);

    public static Uri ToAbsoluteUri(this string url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var uri)
            ? uri
            : throw new ArgumentException($"Url:{url} cannot be used to construct an absolute URI.");
}