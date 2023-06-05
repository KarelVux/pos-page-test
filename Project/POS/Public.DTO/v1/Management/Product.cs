using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Management;

/// <summary>
/// Represents a product data
/// </summary>
public class Product : DomainEntityId
{
    /// <summary>
    /// represents the name of the product
    /// </summary>
    [MaxLength(64)] public string Name { get; set; } = default!;
    /// <summary>
    /// Represents the description of the product
    /// </summary>
    [MaxLength(128)] public string Description { get; set; } = default!;
    /// <summary>
    /// Represents the product unit price
    /// </summary>
    public double UnitPrice { get; set; }
    /// <summary>
    /// Represents the product unit discount
    /// </summary>
    public double UnitDiscount { get; set; }
    /// <summary>
    /// Represents the product unit count
    /// </summary>
    public int UnitCount { get; set; }

    /// <summary>
    /// REpresents the currency of the product
    /// </summary>
    [MaxLength((int)SizeLimits.Currency)]
    public string Currency { get; set; } = default!;

    /// <summary>
    /// Represents the frozen status of the product
    /// </summary>
    public bool Frozen { get; set; }
    
    /// <summary>
    /// Represents the product category id
    /// </summary>
    public Guid ProductCategoryId { get; set; }
    /// <summary>
    /// Represents the business id (aka owner)
    /// </summary>
    public Guid BusinessId { get; set; }
}