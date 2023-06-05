using Domain.Base;

namespace Public.DTO.v1.Manager;

/// <summary>
/// Represents a product picture data
/// </summary>
public class ProductPicture : DomainEntityId
{
    /// <summary>
    /// Represents the picture id
    /// </summary>
    public Guid PictureId { get; set; }
    /// <summary>
    /// Represents the product ID
    /// </summary>
    public Guid ProductId { get; set; }
}