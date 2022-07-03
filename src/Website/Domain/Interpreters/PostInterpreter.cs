using System.Text.RegularExpressions;

namespace Website.Domain.Interpreters;

public class PostInterpreter : IPostInterpreter
{
    private const string RegexPattern = @"(?<={{{0}}})[\s\S]*(?={{{0}}})";

    public string Interpret(string postData, string token)
    {
        var regex = new Regex(GetRegexPattern(token), RegexOptions.Multiline);
        var match = regex.Match(postData);

        return match.Success
            ? match.Value.Trim()
            : string.Empty;
    }

    private string GetRegexPattern(string token) =>
        string.Format(RegexPattern, token);
}