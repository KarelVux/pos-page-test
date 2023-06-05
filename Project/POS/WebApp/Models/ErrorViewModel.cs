namespace WebApp.Models;

/// <summary>
/// Represents error view model
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Represents request id
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Represents show request id
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}