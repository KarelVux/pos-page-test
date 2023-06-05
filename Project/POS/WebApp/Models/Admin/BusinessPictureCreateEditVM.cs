using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.Admin;

/// <summary>
/// Represents business picture create edit view model
/// </summary>
public class BusinessPictureCreateEditVM
{
    /// <summary>
    /// Represents business picture
    /// </summary>
    public BLL.DTO.BusinessPicture BusinessPicture { get; set; } = default!;

    /// <summary>
    /// Represents business select list
    /// </summary>
    public SelectList? BusinessSelectList { get; set; }

    /// <summary>
    /// Represents picture select list
    /// </summary>
    public SelectList? PictureSelectList { get; set; }
}