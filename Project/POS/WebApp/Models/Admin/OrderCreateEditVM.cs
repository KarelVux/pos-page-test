using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.Admin;

/// <summary>
/// Represents order create edit view model
/// </summary>
public class OrderCreateEditVM
{
    /// <summary>
    /// Represents order
    /// </summary>
    public BLL.DTO.Order Order { get; set; } = default!;

    /// <summary>
    /// Represents invoice select list
    /// </summary>
    public SelectList? InvoiceSelectList { get; set; }
}