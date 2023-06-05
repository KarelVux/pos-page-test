using Domain.App.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.Admin;

/// <summary>
/// Represents business manager create edit view model
/// </summary>
public class BusinessManagerCreateEditVM
{
    /// <summary>
    /// Represents business manager
    /// </summary>
    public BLL.DTO.BusinessManager BusinessManager { get; set; } = default!;

    /// <summary>
    /// Represents App user
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