using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.Admin;

/// <summary>
/// Represents business create edit view model
/// </summary>
public class BusinessCreateEditVM
{
    /// <summary>
    /// Represents business
    /// </summary>
    public BLL.DTO.Business Business { get; set; } = default!;

    /// <summary>
    /// Represents business category select list
    /// </summary>
    public SelectList? BusinessCategorySelectList { get; set; }

    /// <summary>
    /// Represents county select list
    /// </summary>
    public SelectList? SettlementSelectList { get; set; }
}