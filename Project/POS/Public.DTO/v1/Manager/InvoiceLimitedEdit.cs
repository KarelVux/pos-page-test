using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Manager;

/// <summary>
/// Represents a limited invoice edit option
/// </summary>
public class InvoiceLimitedEdit : DomainEntityId
{
    /// <summary>
    /// Shows if payment is completed/made
    /// </summary>
    public bool PaymentCompleted { get; set; } = false;
    
    /// <summary>
    /// Represents the invoice acceptance status
    /// </summary>
    public InvoiceAcceptanceStatus InvoiceAcceptanceStatus { get; set; } = InvoiceAcceptanceStatus.Unknown;
    
    //Represent the order acceptance status
    public OrderAcceptanceStatus OrderAcceptanceStatus { get; set; } = OrderAcceptanceStatus.Unknown;

}