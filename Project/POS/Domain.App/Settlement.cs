using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.App;
using Domain.Base;
using Helpers;

namespace Domain.App;

public class Settlement : DomainEntityId
{
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    public string Name { get; set; } = default!;

    public ICollection<Business>? Businesses { get; set; }
}