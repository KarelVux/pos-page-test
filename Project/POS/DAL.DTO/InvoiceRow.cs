using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace DAL.DTO;

public class InvoiceRow : DomainEntityId
{
    public double FinalProductPrice { get; set; }
    public int ProductUnitCount { get; set; }
    public double ProductPricePerUnit { get; set; }
    public double TaxPercent { get; set; }
    public double TaxAmountFromPercent { get; set; }
    

    [MaxLength((int)SizeLimits.Currency)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Currency { get; set; } = default!;

    [MaxLength((int)SizeLimits.Path_MaxLimit)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string? Comment { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public Guid InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }
}