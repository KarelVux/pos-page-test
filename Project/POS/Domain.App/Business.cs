using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Domain.App;

public class Business: DomainEntityId
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public double Rating { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    [MaxLength((int)SizeLimits.MediumLength)]
    public string Address { get; set; }= default!;

    [MaxLength((int)SizeLimits.Name_MaxLength)]

    public string PhoneNumber { get; set; }= default!;

    [MaxLength((int)SizeLimits.Name_MaxLength)]
    public string Email { get; set; }= default!;

    public Guid BusinessCategoryId { get; set; }
    public BusinessCategory? BusinessCategory { get; set; }

    public Guid SettlementId { get; set; }
    public Settlement? Settlement { get; set; }

    public ICollection<BusinessPicture>? BusinessPictures { get; set; }
    public ICollection<Product>? Products { get; set; }
    public ICollection<BusinessManager>? BusinessManagers { get; set; }
    public ICollection<Invoice>? Invoices { get; set; }
}