using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Manager;

/// <summary>
/// Represents a invoiceRow data
/// </summary>
public class InvoiceRow : DomainEntityId
{
    /// <summary>
    /// Represents the products final price
    /// </summary>
    public double FinalProductPrice { get; set; }

    /// <summary>
    /// Represents the amount of items ordered/purchased
    /// </summary>
    public int ProductUnitCount { get; set; }

    /// <summary>
    /// Represents the products price per unit
    /// </summary>
    public double ProductPricePerUnit { get; set; }

    /// <summary>
    /// Represents the products tax percent
    /// </summary>
    public double TaxPercent { get; set; }

    /// <summary>
    /// Represents the Tax amount from percent (tax)
    /// </summary>
    public double TaxAmountFromPercent { get; set; }

    /// <summary>
    /// Represents the prodcut name
    /// </summary>
    public string ProductName { get; set; } = default!;

    /// <summary>
    /// Represents the product currency
    /// </summary>
    [MaxLength((int)SizeLimits.Currency)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Currency { get; set; } = default!;

    /// <summary>
    /// Represents the invoice row comment/optional data
    /// </summary>
    [MaxLength((int)SizeLimits.Path_MaxLimit)]
    public string? Comment { get; set; }

    /// <summary>
    /// Represents the product ID
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Represents the invoice ID
    /// </summary>
    public Guid InvoiceId { get; set; }
}