using System.Text.RegularExpressions;

namespace Website.Services.Validators;

public class PostValidator : IPostValidator
{
    public const string PostNameRegexPattern = @"^[A-Za-z][A-Za-z\d-]*[A-Za-z\d]$";

    public bool IsNameValid(string name)
    {
        var regex = new Regex(PostNameRegexPattern);
        return regex.IsMatch(name);
    }
}