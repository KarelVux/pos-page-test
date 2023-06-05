using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Manager;

/// <summary>
/// Represents a business data.
/// </summary>
public class Business : DomainEntityId
{
    /// <summary>
    /// Represents the name of the business.
    /// </summary>
    [MaxLength((int)SizeLimits.MediumLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Name { get; set; } = default!;

    /// <summary>
    /// Represents the description of the business.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Represents the path/url of the picture
    /// </summary>
    public string? PicturePath { get; set; }

    /// <summary>
    /// Represents the rating of the business.
    /// </summary>
    public double Rating { get; set; }

    /// <summary>
    /// Represents the longitude of the business.
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// Represents the latitude of the business.
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Represents the address of the business.
    /// </summary>
    [MaxLength((int)SizeLimits.MediumLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Address { get; set; } = default!;

    /// <summary>
    /// Represents the phone number of the business
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    [Phone]
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// Represents the email of the business
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    [EmailAddress]
    public string Email { get; set; } = default!;

    /// <summary>
    /// Represents the business category ID
    /// </summary>
    public Guid BusinessCategoryId { get; set; }

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
    /// Represents the products of the business
    /// </summary>
    public ICollection<Product>? Products { get; set; }
}