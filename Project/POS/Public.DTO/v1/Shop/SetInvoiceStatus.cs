using Domain.Base;

namespace Public.DTO.v1.Shop;

/// <summary>
/// Used to set users invoice acceptance status (Main method to change invoice to accepted state)
/// </summary>
public class SetInvoiceStatus : DomainEntityId
{
    /// <summary>
    /// Acceptance status
    /// </summary>
    public bool PropertyStatus { get; set; }
}