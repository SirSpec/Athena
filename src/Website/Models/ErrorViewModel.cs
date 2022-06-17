namespace Website.Models;

public class ErrorViewModel
{
    public string ErrorCode { get; set; } = string.Empty;

    public bool ShowErrorCode =>
        string.IsNullOrWhiteSpace(ErrorCode) is false;

    public string PageTitleSuffix =>
        ShowErrorCode ? $" - {ErrorCode}" : string.Empty;
}