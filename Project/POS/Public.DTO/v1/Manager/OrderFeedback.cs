using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Manager;

/// <summary>
/// Represents a order feedback data
/// </summary>
public class OrderFeedback: DomainEntityId
{
    /// <summary>
    /// Represents the order feedback title
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    public string Title { get; set; } = default!;

    /// <summary>
    /// Represents the order feedback description
    /// </summary>
    [MaxLength((int)SizeLimits.Description_MaxLength)]
    public string? Description { get; set; }

    /// <summary>
    /// Represents the order rating
    /// </summary>
    [Range((int)SizeLimits.Zero, 10)]
    public double Rating { get; set; }

    /// <summary>
    /// Represents the order id (feedback owner
    /// </summary>
    public Guid OrderId { get; set; }
    
    /// <summary>
    /// Represents the order
    /// </summary>
    public Order? Order { get; set; }
}