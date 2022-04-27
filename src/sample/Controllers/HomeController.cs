using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using otel_code_path.Models;

namespace otel_code_path.Controllers;

public class HomeController : Controller
{
    private static ActivitySource _default = new ActivitySource("MySource");
    
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        using var span = _default.StartActivity("New Activity");
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
