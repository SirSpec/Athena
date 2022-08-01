using System.Text.RegularExpressions;
using System.Web;

namespace Microsoft.AspNetCore.Html;

public static class HtmlStringExtensions
{
    public static HtmlString ToHtmlString(this string @string) =>
        new HtmlString(@string);

    public static HtmlString ToHtmlStringWithCodeBlocks(this string @string)
    {
        var output = @string;
        foreach (Match codeBlock in GetCodeBlockMatches(@string))
        {
            var escapedCodeBlock = HttpUtility.HtmlEncode(codeBlock.Value);
            output = output.Replace(codeBlock.Value, escapedCodeBlock);
        }

        return output.ToHtmlString();
    }

    private static MatchCollection GetCodeBlockMatches(string @string)
    {
        const string CodeBlockRegexPattern = @"(?<=<code[^>]*>)(.|\n)*?(?=<\/code>)";
        return Regex.Matches(@string, CodeBlockRegexPattern);
    }
}