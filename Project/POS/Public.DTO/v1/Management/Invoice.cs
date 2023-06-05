using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Management;

/// <summary>
/// Represents an invoice data.
/// </summary>
public class Invoice : DomainEntityId
{
    /// <summary>
    /// Represents final price of the invoice
    /// </summary>
    public double FinalTotalPrice { get; set; }

    /// <summary>
    /// Represents tax amount of the invoice
    /// </summary>
    public double TaxAmount { get; set; }

    /// <summary>
    /// Represents total price without tax of the invoice
    /// </summary>
    public double TotalPriceWithoutTax { get; set; }

    /// <summary>
    /// Represents if the payment is completed
    /// </summary>
    public bool PaymentCompleted { get; set; } = false;

    /// <summary>
    /// Represents the invoice acceptance status
    /// </summary>
    public InvoiceAcceptanceStatus InvoiceAcceptanceStatus { get; set; } = InvoiceAcceptanceStatus.Unknown;

    /// <summary>
    /// Represents the creation time of the invoice
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// Represents the user ID of the invoice
    /// </summary>
    public Guid AppUserId { get; set; }

    /// <summary>
    /// Represents the business ID of the invoice
    /// </summary>
    public Guid BusinessId { get; set; }

    /// <summary>
    /// Represents the order ID of the invoice.
    /// </summary>
    public Guid? OrderId { get; set; }
}