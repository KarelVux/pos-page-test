using Domain.App.Identity;
using Domain.Base;
using Helpers;

namespace Domain.App;

public class Invoice : DomainEntityId
{
    public double FinalTotalPrice { get; set; }
    public double TaxAmount { get; set; }
    public double TotalPriceWithoutTax { get; set; }
    public bool PaymentCompleted { get; set; } = false;
    public InvoiceAcceptanceStatus InvoiceAcceptanceStatus { get; set; } = InvoiceAcceptanceStatus.Unknown;
    public DateTime CreationTime { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public Guid BusinessId { get; set; }
    public Business? Business { get; set; }

    public Guid? OrderId { get; set; }
    public Order? Order { get; set; }

    public ICollection<InvoiceRow>? InvoiceRows { get; set; }
}