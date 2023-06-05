using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace DAL.DTO;

public class Settlement : DomainEntityId
{
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    public string Name { get; set; } = default!;

    public ICollection<Business>? Businesses { get; set; }
}