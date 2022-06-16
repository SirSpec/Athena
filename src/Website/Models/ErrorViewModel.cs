namespace Website.Models;

public class ErrorViewModel
{
    public string? ErrorCode { get; set; }

    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
