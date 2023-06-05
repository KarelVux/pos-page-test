using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Manager;

/// <summary>
/// Represents a picture data
/// </summary>
public class Picture : DomainEntityId
{
    /// <summary>
    /// represents the picture title (image name)
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Title { get; set; } = default!;

    /// <summary>
    /// Represents the picture description
    /// </summary>
    [MaxLength((int)SizeLimits.Description_MaxLength)]
    public string? Description { get; set; }

    /// <summary>
    /// Represents the picture path/url
    /// </summary>
    [MaxLength((int)SizeLimits.Path_MaxLimit)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Path { get; set; } = default!;
}