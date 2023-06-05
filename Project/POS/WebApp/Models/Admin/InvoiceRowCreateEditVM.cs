using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.Admin;

/// <summary>
/// Represents invoice row create edit view model
/// </summary>
public class InvoiceRowCreateEditVM
{
    /// <summary>
    /// Represents invoice row
    /// </summary>
    public BLL.DTO.InvoiceRow InvoiceRow { get; set; } = default!;

    /// <summary>
    /// Represents invoice select list
    /// </summary>
    public SelectList? InvoiceSelectList { get; set; }

    /// <summary>
    /// Represents product select list
    /// </summary>
    public SelectList? ProductSelectList { get; set; }
}