namespace Public.DTO.v1.Shop;

/// <summary>
/// Used to display product and unit count that is ordered
/// </summary>
public class InvoiceCreateEditProduct
{
    /// <summary>
    /// Product ID that will be ordered
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Displays how many will products will be ordered
    /// </summary>
    public int ProductUnitCount { get; set; }
}