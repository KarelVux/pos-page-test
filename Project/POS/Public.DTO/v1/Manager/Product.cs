using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Manager;

/// <summary>
/// Represents a product data
/// </summary>
public class Product : DomainEntityId
{
    /// <summary>
    /// Represents the product name
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// Represents the product picture path/url
    /// </summary>
    public string? PicturePath { get; set; }

    /// <summary>
    /// Represents the product description
    /// </summary>
    [MaxLength((int)SizeLimits.MediumLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Description { get; set; } = default!;

    /// <summary>
    /// Represents the product price
    /// </summary>
    [Range((int)SizeLimits.Zero, 9000)]
    public double UnitPrice { get; set; }
    
    /// <summary>
    /// Represents the product discount amount
    /// </summary>
    [Range(-9999, (int)SizeLimits.Zero)] public double UnitDiscount { get; set; }
    
    [Range( (int)SizeLimits.Zero, 10000)]
    public int UnitCount { get; set; }
    
    /// <summary>
    /// Represents the product tax percent
    /// </summary>
    [Range( (int)SizeLimits.Zero, 100)]
    public double TaxPercent { get; set; } = default!;

    /// <summary>
    /// Represents the product currency
    /// </summary>
    [MaxLength((int)SizeLimits.Currency)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Currency { get; set; } = default!;

    /// <summary>
    /// Represents the product frozen state
    /// </summary>
    public bool Frozen { get; set; }
    public Guid ProductCategoryId { get; set; }
    public ProductCategory? ProductCategory { get; set; }

    public Guid BusinessId { get; set; }
}