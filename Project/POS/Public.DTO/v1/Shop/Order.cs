using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Shop;

/// <summary>
/// Represents a order data
/// </summary>
public class Order : DomainEntityId
{
    /// <summary>
    /// Represents the order start time (created time)
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Represents the order end time (completed time)
    /// </summary>
    public DateTime GivenToClientTime { get; set; }

    /// <summary>
    /// Represents the order acceptance status
    /// </summary>
    public OrderAcceptanceStatus OrderAcceptanceStatus { get; set; } = OrderAcceptanceStatus.Unknown;

    /// <summary>
    /// Represents customer comment
    /// </summary>

    [MaxLength((int)SizeLimits.Description_MaxLength)]
    public string? CustomerComment { get; set; }

    /// <summary>
    /// Represents the invoice ID
    /// </summary>

    public Guid InvoiceId { get; set; }
}