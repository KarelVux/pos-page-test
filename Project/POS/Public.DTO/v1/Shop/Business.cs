using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Shop;

/// <summary>
///  Used to display business info
/// </summary>
public class Business : DomainEntityId
{
    /// <summary>
    /// Represents the business name
    /// </summary>
    [MaxLength((int)SizeLimits.MediumLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Name { get; set; } = default!;

    /// <summary>
    /// Represents the business description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Represents the business picture path/url
    /// </summary>
    public string? PicturePath { get; set; }

    /// <summary>
    /// Represents the business rating
    /// </summary>
    public double Rating { get; set; }

    /// <summary>
    /// Represents the business longitude 
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// Represents the business latitude
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Represents the business address
    /// </summary>

    [MaxLength((int)SizeLimits.MediumLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Address { get; set; } = default!;


    /// <summary>
    /// Represents the business phone number
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    [Phone]
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// Represents the business email
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    [EmailAddress]
    public string Email { get; set; } = default!;

    /// <summary>
    /// Represents the business category ID
    /// </summary>
    public Guid BusinessCategoryId { get; set; }

    /// <summary>
    /// Represents the business category
    /// </summary>
    public BusinessCategory? BusinessCategory { get; set; }

    /// <summary>
    /// Represents the settlement ID
    /// </summary>
    public Guid SettlementId { get; set; }

    /// <summary>
    /// Represents the settlement
    /// </summary>
    public Settlement? Settlement { get; set; }

    /// <summary>
    /// Represents the business products
    /// </summary>
    public ICollection<Product>? Products { get; set; }
}