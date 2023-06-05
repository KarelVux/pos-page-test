using Domain.App;
using Domain.App.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.Admin;

/// <summary>
/// Represents invoice create edit view model
/// </summary>
public class InvoiceCreateEditVM
{
    /// <summary>
    /// Represents invoice
    /// </summary>
    public BLL.DTO.Invoice Invoice { get; set; } = default!;

    /// <summary>
    /// Represents business app user
    /// </summary>
    public AppUser? AppUser { get; set; }

    /// <summary>
    /// Represents app user select list
    /// </summary>
    public SelectList? AppUserSelectList { get; set; }

    /// <summary>
    /// Represents business select list
    /// </summary>
    public SelectList? BusinessSelectList { get; set; }
}