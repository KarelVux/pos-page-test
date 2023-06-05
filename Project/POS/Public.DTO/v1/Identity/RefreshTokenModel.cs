namespace Public.DTO.v1.Identity;

/// <summary>
/// Represents a request to refresh a JWT
/// </summary>
public class RefreshTokenModel
{
    /// <summary>
    /// JWT  token
    /// </summary>
    public string Jwt { get; set; } = default!;
    
    /// <summary>
    /// Refresh token value
    /// </summary>
    public string RefreshToken { get; set; } = default!;
}