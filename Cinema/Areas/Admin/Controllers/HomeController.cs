using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cinema.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "admin")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.SuccessMessage = TempData["SucessAdminLogin"];
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
