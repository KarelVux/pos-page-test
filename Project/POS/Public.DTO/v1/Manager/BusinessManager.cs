using Domain.Base;

namespace Public.DTO.v1.Manager;

/// <summary>
/// Represents a business manager data (aka. the owner of the business)
/// </summary>
public class BusinessManager : DomainEntityId
{
    /// <summary>
    /// Represents the app user id (aka owner)
    /// </summary>
    public Guid AppUserId { get; set; }

    /// <summary>
    /// Represents the business id 
    /// </summary>
    public Guid BusinessId { get; set; }
}