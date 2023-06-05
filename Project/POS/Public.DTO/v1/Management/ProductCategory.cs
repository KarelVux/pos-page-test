using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Management;

/// <summary>
/// Represents a product category data
/// </summary>
public class ProductCategory: DomainEntityId
{
    /// <summary>
    /// Represents the title of the product category
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Title { get; set; } = default!;
}