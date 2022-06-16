using Microsoft.AspNetCore.Html;

namespace Website.Extensions;

public static class StringExtensions
{
    public static HtmlString ToHtmlString(this string @string) =>
        new HtmlString(@string);
}