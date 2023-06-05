using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.Admin;

/// <summary>
/// Represents product create edit view model
/// </summary>
public class ProductCreateEditVM
{
    /// <summary>
    /// Represents product
    /// </summary>
    public BLL.DTO.Product Product { get; set; } = default!;

    /// <summary>
    /// Represents business select list
    /// </summary>
    public SelectList? BusinessSelectList { get; set; }

    /// <summary>
    /// Represents product category select list
    /// </summary>
    public SelectList? ProductCategorySelectList { get; set; }
}