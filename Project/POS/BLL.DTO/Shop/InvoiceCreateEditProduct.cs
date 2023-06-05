namespace BLL.DTO.Shop;

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