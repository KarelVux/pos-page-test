using BLL.DTO.Identity;
using Domain.Base;
using Domain.Contracts.Base;

namespace Public.DTO.v1.Identity;

/// <summary>
/// Represents an application refresh token
/// </summary>
public class AppRefreshToken : BaseRefreshToken, IDomainEntityId
{
    /// <summary>
    /// Reference to the user who owns this refresh token
    /// </summary>
    public Guid AppUserId { get; set; }
}