using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models.Admin;

/// <summary>
/// Represents order feedback create edit view model
/// </summary>
public class OrderFeedbackCreateEditVM
{
    /// <summary>
    /// Represents order feedback
    /// </summary>
    public BLL.DTO.OrderFeedback OrderFeedback { get; set; } = default!;

    /// <summary>
    /// Represents order select list
    /// </summary>
    public SelectList? OrderSelectList { get; set; }
}