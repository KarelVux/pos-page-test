using System.ComponentModel.DataAnnotations;
using Helpers;

namespace Public.DTO.v1.Shop;

/// <summary>
/// Represents a invoice row
/// </summary>
public class InvoiceRow
{
    /// <summary>
    /// Represents the invoice row id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Represents the product name
    /// </summary>
    public string ProductName { get; set; } = default!;

    /// <summary>
    /// Represents the final product price
    /// </summary>
    public double FinalProductPrice { get; set; }

    /// <summary>
    /// Represents product unit count
    /// </summary>
    public int ProductUnitCount { get; set; }

    /// <summary>
    /// Represents the product price per unit
    /// </summary>
    public double ProductPricePerUnit { get; set; }

    /// <summary>
    /// Represents the tax present of the product
    /// </summary>
    public double TaxPercent { get; set; }

    /// <summary>
    /// Represents the tax amount from percent
    /// </summary>
    public double TaxAmountFromPercent { get; set; }

    /// <summary>
    /// Represents product ID
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Represents the currenty
    /// </summary>

    [MaxLength((int)SizeLimits.Currency)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Currency { get; set; } = default!;

    /// <summary>
    /// Represents the invoice row comment
    /// </summary>
    [MaxLength((int)SizeLimits.Path_MaxLimit)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string? Comment { get; set; }
}