using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Helpers;

namespace Public.DTO.v1.Shop;

// used to display BusinessCategories
public class BusinessCategory : DomainEntityId
{
    /// <summary>
    /// Represents the business category title
    /// </summary>
    [MaxLength((int)SizeLimits.Name_MaxLength)]
    public string Title { get; set; } = default!;
}