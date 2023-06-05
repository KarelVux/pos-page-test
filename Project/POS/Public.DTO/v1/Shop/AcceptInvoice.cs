namespace Public.DTO.v1.Shop;

/// <summary>
/// Represents a user acceptance (client) of an invoice
/// </summary>
public class AcceptInvoice
{
    
    /// <summary>
    /// User acceptance
    /// true = user accepts teh invoice
    /// false = user rejects 
    /// </summary>
    public bool Acceptance { get; set; }
}