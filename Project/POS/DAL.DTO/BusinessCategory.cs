using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace DAL.DTO;

public class BusinessCategory : DomainEntityId
{
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Title { get; set; } = default!;

    public ICollection<Business>? Businesses { get; set; }
}