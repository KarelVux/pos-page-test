using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Manager;

/// <summary>
/// Represents a settlement data
/// </summary>
public class Settlement : DomainEntityId
{
    /// <summary>
    /// Represents the settlement name
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    public string Name { get; set; } = default!;
}