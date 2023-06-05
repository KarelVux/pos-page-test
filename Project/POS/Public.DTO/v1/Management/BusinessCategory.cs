using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Management;

/// <summary>
/// Represents a business category
/// </summary>
public class BusinessCategory : DomainEntityId
{
    /// <summary>
    /// reference to the business category name
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Title { get; set; } = default!;
}