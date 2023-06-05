using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace BLL.DTO;

public class Product : DomainEntityId
{
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Name { get; set; } = default!;

    [MaxLength((int)SizeLimits.MediumLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Description { get; set; } = default!;
    public string? PicturePath { get; set; }

    public double UnitPrice { get; set; }
    [Range(-9999, (int)SizeLimits.Zero)]   public double UnitDiscount { get; set; }
    public int UnitCount { get; set; }
    public double TaxPercent { get; set; } = default!;

    [MaxLength((int)SizeLimits.Currency)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Currency { get; set; } = default!;

    public bool Frozen { get; set; }
    public Guid ProductCategoryId { get; set; }
    public ProductCategory? ProductCategory { get; set; }

    public Guid BusinessId { get; set; }
    public Business? Business { get; set; }

    public ICollection<InvoiceRow>? InvoiceRows { get; set; }
    public ICollection<ProductPicture>? ProductPicture { get; set; }
}