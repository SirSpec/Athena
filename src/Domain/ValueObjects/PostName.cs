using System.Text.RegularExpressions;

namespace Athena.Domain.ValueObjects;

public record PostName
{
    public const string NameFormatRegexPattern = @"^[A-Za-z][A-Za-z\d-]*[A-Za-z\d]$";

    public string Value { get; }

    public PostName(string value) =>
        Value = IsValidFormat(value)
            ? value
            : throw new ArgumentException($"Post name:{value} is invalid.");

    public static bool IsValidFormat(string name) =>
        Regex.IsMatch(name, NameFormatRegexPattern);

    public static implicit operator string(PostName postName) =>
        postName.Value;

    public static explicit operator PostName(string value) =>
        new PostName(value);
}