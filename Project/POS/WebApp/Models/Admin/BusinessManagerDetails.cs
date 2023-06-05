namespace WebApp.Models.Admin;

/// <summary>
/// Represents business manager details view model
/// </summary>
public class BusinessManagerDetails
{
    /// <summary>
    /// Represents business manager
    /// </summary>
    public BLL.DTO.BusinessManager BusinessManager { get; set; } = default!;

    /// <summary>
    /// Represents apps username
    /// </summary>
    public string? UserName { get; set; } 
}