using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;
using Helpers;

namespace Domain.App;

public class OrderFeedback: DomainEntityId
{
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    public string Title { get; set; } = default!;

    [MaxLength((int)SizeLimits.Description_MaxLength)]
    public string? Description { get; set; }

    public double Rating { get; set; }

    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
}