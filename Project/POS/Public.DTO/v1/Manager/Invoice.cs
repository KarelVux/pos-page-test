using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Manager;

/// <summary>
/// Represents an invoice
/// </summary>
public class Invoice : DomainEntityId
{
    /// <summary>
    /// Represents final price 
    /// </summary>
    public double FinalTotalPrice { get; set; }

    /// <summary>
    /// Represents total tax amount 
    /// </summary>
    public double TaxAmount { get; set; }

    /// <summary>
    /// Represents total price without tax 
    /// </summary>
    public double TotalPriceWithoutTax { get; set; }

    /// <summary>
    /// Represents if the payment is completed
    /// </summary>
    public bool PaymentCompleted { get; set; } = false;

    /// <summary>
    /// Represents the invoice acceptance status
    /// </summary>
    public InvoiceAcceptanceStatus InvoiceAcceptanceStatus { get; set; } = 0;

    /// <summary>
    /// Represents the creation time of the invoice
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// Represents the user ID of the invoice (aka the customer)
    /// </summary>
    public Guid AppUserId { get; set; }

    /// <summary>
    /// Represents the user name of the invoice (aka the customer usena,e)
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Represents the business ID (aka. the seller)
    /// </summary>
    public Guid BusinessId { get; set; }

    /// <summary>
    /// Represents the business 
    /// </summary>
    public Business? Business { get; set; }

    /// <summary>
    /// Represents the order ID of the invoice
    /// </summary>
    public Guid? OrderId { get; set; }

    /// <summary>
    /// Represents the order of the invoice
    /// </summary>
    public Order? Order { get; set; }

    /// <summary>
    /// Represents the invoice rows of the invoice (aka product price when the invoice was created)
    /// </summary>
    public ICollection<InvoiceRow>? InvoiceRows { get; set; }
}