using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace BLL.DTO;

public class Settlement : DomainEntityId
{
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    public string Name { get; set; } = default!;
}