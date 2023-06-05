using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Management;

/// <summary>
/// Represents a settlement data
/// </summary>
public class Settlement : DomainEntityId
{
    /// <summary>
    /// Represents the name of the settlement
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    public string Name { get; set; } = default!;
}