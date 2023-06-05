using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Shop;

/// <summary>
///  Used to display settlement info
/// </summary>
public class Settlement : DomainEntityId
{
    /// <summary>
    /// Settlement name
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    public string Name { get; set; } = default!;
}