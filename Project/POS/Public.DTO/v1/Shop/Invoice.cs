using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Shop;

/// <summary>
/// Represents an invoice data
/// </summary>
public class Invoice : DomainEntityId
{
    /// <summary>
    /// Represents the final total price of the invoice
    /// </summary>
    public double FinalTotalPrice { get; set; }

    /// <summary>
    /// Represents total tax
    /// </summary>
    public double TaxAmount { get; set; }

    /// <summary>
    /// Represents total price without tax
    /// </summary>
    public double TotalPriceWithoutTax { get; set; }

    /// <summary>
    /// Shows if the payment is completed
    /// </summary>
    public bool PaymentCompleted { get; set; } = false;

    /// <summary>
    /// Represents the invoice acceptance status
    /// </summary>
    public InvoiceAcceptanceStatus InvoiceAcceptanceStatus { get; set; } = InvoiceAcceptanceStatus.Unknown;

    /// <summary>
    /// Represents the creation time
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// Represents the business id
    /// </summary>
    public Guid BusinessId { get; set; }

    /// <summary>
    /// Represents the business name
    /// </summary>
    public string? BusinessName { get; set; }

    /// <summary>
    /// Represents the the OrderId
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Represents the order
    /// </summary>
    public Public.DTO.v1.Shop.Order? Order { get; set; }

    /// <summary>
    /// Represents the invoice rows
    /// </summary>
    public List<InvoiceRow>? InvoiceRows { get; set; }
}