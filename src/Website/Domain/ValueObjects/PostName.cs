using System.Text.RegularExpressions;

namespace Website.Domain.ValueObjects;

public record PostName
{
    public const string ValueRegexPattern = @"^[A-Za-z][A-Za-z\d-]*[A-Za-z\d]$";

    public string Value { get; }

    public PostName(string value) =>
        Value = IsNameValid(value)
            ? value
            : throw new ArgumentException($"Post name:{value} is invalid.");

    public static bool IsNameValid(string name) =>
        Regex.IsMatch(name, ValueRegexPattern);
}