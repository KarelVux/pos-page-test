using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Management;

/// <summary>
/// Represents a picture data
/// </summary>
public class Picture : DomainEntityId
{
    /// <summary>
    /// Represents the title of the picture (Picture name)
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Title { get; set; } = default!;

    /// <summary>
    /// represents the description of the picture
    /// </summary>
    [MaxLength((int)SizeLimits.Description_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string? Description { get; set; }

    /// <summary>
    /// Represents the path/url of the picture
    /// </summary>
    [MaxLength((int)SizeLimits.Path_MaxLimit)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Path { get; set; } = default!;
}