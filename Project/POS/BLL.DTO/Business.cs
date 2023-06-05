using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace BLL.DTO;

public class Business: DomainEntityId
{
    [MaxLength((int)SizeLimits.MediumLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? PicturePath { get; set; }
    public double Rating { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    [MaxLength((int)SizeLimits.MediumLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Address { get; set; }= default!;


    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    [Phone]
    public string PhoneNumber { get; set; }= default!;


    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    [EmailAddress]
    public string Email { get; set; }= default!;

    public Guid BusinessCategoryId { get; set; }
    public BusinessCategory? BusinessCategory { get; set; }

    public Guid SettlementId { get; set; }
    public Settlement? Settlement { get; set; }

    public ICollection<Product>? Products { get; set; }
}