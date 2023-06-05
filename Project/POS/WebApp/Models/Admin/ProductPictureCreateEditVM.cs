using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.Admin;

/// <summary>
/// Represents product picture create edit view model
/// </summary>
public class ProductPictureCreateEditVM
{
    /// <summary>
    /// Represents product picture
    /// </summary>
    public BLL.DTO.ProductPicture ProductPicture { get; set; } = default!;

    /// <summary>
    /// Represents picture select list
    /// </summary>
    public SelectList? PictureSelectList { get; set; }
    /// <summary>
    /// Represents product select list
    /// </summary>
    public SelectList? ProductSelectList { get; set; }
}