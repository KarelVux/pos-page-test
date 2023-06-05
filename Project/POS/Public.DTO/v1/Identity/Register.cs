using System.ComponentModel.DataAnnotations;

namespace Public.DTO.v1.Identity;

/// <summary>
/// Represents a request to register
/// </summary>
public class Register
{
    /// <summary>
    ///    User email for login
    /// </summary>
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Incorrect length")]
    public string Email { get; set; } = default!;

    /// <summary>
    /// User password for login
    /// </summary>
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Incorrect length")]
    public string Password { get; set; } = default!;
}