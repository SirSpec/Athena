namespace Athena.Website.Models;

public class ErrorViewModel
{
    public string ErrorCode { get; init; } = string.Empty;

    public bool ShowErrorCode =>
        string.IsNullOrWhiteSpace(ErrorCode) is false;

    public string PageTitleSuffix =>
        ShowErrorCode ? $" - {ErrorCode}" : string.Empty;
}