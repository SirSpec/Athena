namespace System;

public static class UriExtensions
{
    public static Uri ToAbsoluteUri(this string url) =>
        Uri.IsWellFormedUriString(url, UriKind.Absolute) && Uri.TryCreate(url, UriKind.Absolute, out var uri)
            ? uri
            : throw new ArgumentException($"Url:{url} cannot be used to construct an absolute URI.");
}