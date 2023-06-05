using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Management;

/// <summary>
/// Used to represent a business
/// </summary>
public class Business : DomainEntityId
{
    
    /// <summary>
    /// reference to the business name
    /// </summary>
    public string Name { get; set; } = default!;
    /// <summary>
    /// Reference to the business description
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Reference to the business rating
    /// </summary>
    public double Rating { get; set; }
    
    /// <summary>
    /// Reference to the business longitude 
    /// </summary>
    public double Longitude { get; set; }
    
    /// <summary>
    /// Reference to the business latitude
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Reference to the business address
    /// </summary>
    [MaxLength((int)SizeLimits.MediumLength)]
    public string Address { get; set; } = default!;

    /// <summary>
    /// Reference to the business phone number
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// Reference to the business email
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    public string Email { get; set; } = default!;

    /// <summary>
    /// Reference to the business category ID
    /// </summary>
    public Guid BusinessCategoryId { get; set; }

    /// <summary>
    /// Reference to the business settlement ID
    /// </summary>
    public Guid SettlementId { get; set; }
}