using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Manager;

/// <summary>
/// Represents a order data
/// </summary>
public class Order : DomainEntityId
{
    /// <summary>
    /// Represents the order start time
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Represents the when order was given to client
    /// </summary>
    public DateTime GivenToClientTime { get; set; }

    /// <summary>
    /// Represents the order status
    /// </summary>
    public OrderAcceptanceStatus OrderAcceptanceStatus { get; set; } = OrderAcceptanceStatus.Unknown;


    /// <summary>
    /// Represents the customer optional comment 
    /// </summary>
    [MaxLength((int)SizeLimits.Description_MaxLength)]
    public string? CustomerComment { get; set; }
}