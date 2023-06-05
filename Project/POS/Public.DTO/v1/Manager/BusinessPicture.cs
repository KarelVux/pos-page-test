using Domain.Base;

namespace Public.DTO.v1.Manager;

/// <summary>
/// Represents a business picture data (aka. the picture of the business).
/// </summary>
public class BusinessPicture : DomainEntityId
{
    /// <summary>
    /// Represents the picture id
    /// </summary>
    public Guid PictureId { get; set; }
    /// <summary>
    /// Represents the business id
    /// </summary>
    public Guid BusinessId { get; set; }
}