using Domain.App;
using Domain.App.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.Admin;

/// <summary>
/// Represents invoice details view model
/// </summary>
public class InvoiceDetails
{
    /// <summary>
    /// Represents invoice
    /// </summary>
    public BLL.DTO.Invoice Invoice { get; set; } = default!;

    /// <summary>
    /// Represents app username
    /// </summary>
    public string? UserName { get; set; }
}