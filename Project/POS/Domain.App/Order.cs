using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Domain.App;

public class Order : DomainEntityId
{
    public DateTime StartTime { get; set; }
    public DateTime GivenToClientTime { get; set; }
    public OrderAcceptanceStatus OrderAcceptanceStatus { get; set; } = OrderAcceptanceStatus.Unknown;

    [MaxLength((int)SizeLimits.Description_MaxLength)]
    public string? CustomerComment { get; set; }

    
    public Invoice? Invoice { get; set; }

    public OrderFeedback? OrderFeedback { get; set; }
}