using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Shop;

/// <summary>
/// Used to display ProductInfo
/// </summary>
public class Product : DomainEntityId
{
    /// <summary>
    /// Represents the product name
    /// </summary>
    [MaxLength(64)]
    public string Name { get; set; } = default!;

    /// <summary>
    /// Represents the product description
    /// </summary>
    [MaxLength(128)]
    public string Description { get; set; } = default!;

    /// <summary>
    /// Represents the product price
    /// </summary>
    public double UnitPrice { get; set; }

    /// <summary>
    /// Represents the product picture path/url
    /// </summary>
    public string? PicturePath { get; set; }

    /// <summary>
    /// Represents the product discount per unit
    /// </summary>
    public double UnitDiscount { get; set; }

    /// <summary>
    /// Represents the product unit count
    /// </summary>
    public int UnitCount { get; set; }

    /// <summary>
    /// Represents the product tax percent
    /// </summary>
    public double TaxPercent { get; set; } = default!;

    /// <summary>
    /// Represents the product currency
    /// </summary>
    [MaxLength((int)SizeLimits.Currency)]
    public string Currency { get; set; } = default!;

    /// <summary>
    /// Represents the product frozen state
    /// </summary>
    public bool Frozen { get; set; }

    /// <summary>
    /// Represents the product category id
    /// </summary>
    public Guid ProductCategoryId { get; set; }

    /// <summary>
    /// Represents the product category
    /// </summary>
    public DTO.v1.Shop.ProductCategory ProductCategory { get; set; } = null!;

    /// <summary>
    /// Represents the business id
    /// </summary>
    public Guid BusinessId { get; set; }
}