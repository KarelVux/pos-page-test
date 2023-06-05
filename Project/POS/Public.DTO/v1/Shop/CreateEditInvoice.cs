using System.Linq.Expressions;
using Domain.Base;

namespace Public.DTO.v1.Shop;

/// <summary>
///  Used to generate a invoice for the order
/// </summary>
public class CreateEditInvoice : DomainEntityId
{
    /// <summary>
    /// Business ID where the order is made
    /// </summary>
    public Guid BusinessId { get; set; }

    /// <summary>
    /// List of products to be ordered
    /// </summary>
    public IEnumerable<Public.DTO.v1.Shop.InvoiceCreateEditProduct> InvoiceCreateEditProducts { get; set; } = default!;
}