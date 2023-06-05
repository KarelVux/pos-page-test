namespace Public.DTO.v1.Identity;

/// <summary>
/// Represents a response from the server containing a JWT and a refresh token
/// </summary>
public class JWTResponse
{
    /// <summary>
    /// JWT token
    /// </summary>
    public string JWT { get; set; } = default!;
    
    /// <summary>
    /// Refresh token value
    /// </summary>
    public string RefreshToken { get; set; } = default!;
}