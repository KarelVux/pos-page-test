namespace Public.DTO.v1.Identity;

/// <summary>
/// Represents a request to log out
/// </summary>
public class Logout
{
    /// <summary>
    /// Logout refresh token
    /// </summary>
    public string RefreshToken { get; set; } = default!;
}