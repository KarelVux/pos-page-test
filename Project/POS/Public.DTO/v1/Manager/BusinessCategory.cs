using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Manager;

/// <summary>
/// Represents a business category data
/// </summary>
public class BusinessCategory : DomainEntityId
{
    /// <summary>
    /// Represents the title of the business category
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Title { get; set; } = default!;

}