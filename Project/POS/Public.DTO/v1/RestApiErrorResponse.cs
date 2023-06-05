using System.Net;

namespace Public.DTO.v1;

/// <summary>
/// Represents the error response from the API
/// </summary>
public class RestApiErrorResponse
{
    /// <summary>
    /// Represents the status code of the response
    /// </summary>
    public HttpStatusCode Status { get; set; }
    /// <summary>
    /// Represents the error message
    /// </summary>
    public string Error { get; set; } = default!;
}