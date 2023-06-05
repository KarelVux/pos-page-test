using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Domain.Base;
using Helpers;

namespace Domain.App;

public class Picture : DomainEntityId
{
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string Title { get; set; } = default!;

    [MaxLength((int)SizeLimits.Description_MaxLength)]
    [MinLength((int)SizeLimits.Default_MinLength)]
    public string? Description { get; set; }

    [MaxLength((int)SizeLimits.Path_MaxLimit)]
    public string Path { get; set; } = default!;

    public ICollection<BusinessPicture>? BusinessPictures { get; set; }
    public ICollection<ProductPicture>? ProductPictures { get; set; }
}