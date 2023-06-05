using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Management;

/// <summary>
///  Represents an order data
/// </summary>
public class Order : DomainEntityId
{
    /// <summary>
    /// Represents the creation time of the order
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Represents the given to client time of the order
    /// </summary>
    public DateTime GivenToClientTime { get; set; }

    /// <summary>
    /// Represents the order acceptance status
    /// </summary>
    public OrderAcceptanceStatus OrderAcceptanceStatus { get; set; } = OrderAcceptanceStatus.Unknown;

    /// <summary>
    /// Represents the customer comment of the order
    /// </summary>
    [MaxLength((int)SizeLimits.Description_MaxLength)]
    public string? CustomerComment { get; set; }
}