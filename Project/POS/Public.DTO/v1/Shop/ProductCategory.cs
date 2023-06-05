using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Shop;

/// <summary>
/// Used to display ProductCategory info
/// </summary>
public class ProductCategory : DomainEntityId

{
    /// <summary>
    /// Represents the product category title
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    public string Title { get; set; } = default!;
}