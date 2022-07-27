using System.Text.RegularExpressions;

namespace Athena.Domain.ValueObjects;

public record PostBody
{
    private static class PostBodyTokens
    {
        public const string Title = nameof(Title);
        public const string PublishingDate = nameof(PublishingDate);
        public const string Description = nameof(Description);
        public const string Body = nameof(Body);
    }

    public PostBody(string postData)
    {
        PublishingDate = Extract(postData, PostBodyTokens.PublishingDate);
        Title = Extract(postData, PostBodyTokens.Title);
        Description = Extract(postData, PostBodyTokens.Description);
        Body = Extract(postData, PostBodyTokens.Body);
    }

    public string PublishingDate { get; }
    public string Title { get; }
    public string Description { get; }
    public string Body { get; }

    private static string Extract(string postData, string token)
    {
        var regex = new Regex(GetTextInsideRegexPattern(token), RegexOptions.Multiline);
        var match = regex.Match(postData);

        return match.Success
            ? match.Value.Trim()
            : string.Empty;
    }

    private static string GetTextInsideRegexPattern(string token)
    {
        const string TextInsideRegexPattern = @"(?<={{{0}}})[\s\S]*(?={{{0}}})";
        return string.Format(TextInsideRegexPattern, token);
    }
}