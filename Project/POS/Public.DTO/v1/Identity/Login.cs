using System.ComponentModel.DataAnnotations;

namespace Public.DTO.v1.Identity;

/// <summary>
/// Represents a request to  login
/// </summary>
public class Login
{
    /// <summary>
    ///     User email for login
    /// </summary>
    [StringLength(maximumLength: 128, MinimumLength = 5, ErrorMessage = "Wrong length on email")]
    public string Email { get; set; } = default!;

    /// <summary>
    /// User password for login
    /// </summary>
    public string Password { get; set; } = default!;
}