using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

/// <summary>
/// Represents home controller
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// represents logger
    /// </summary>
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Represents HomeController
    /// </summary>
    /// <param name="logger">Ilogger</param>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Represents index 
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Represents privacy 
    /// </summary>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Represents error
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}