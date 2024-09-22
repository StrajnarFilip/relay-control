using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proculite.RelayControl.Models;
using Proculite.RelayControl.Services;

namespace Proculite.RelayControl.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public readonly PinService _pinService;

    public HomeController(ILogger<HomeController> logger, PinService pinService)
    {
        _logger = logger;
        _pinService = pinService;
    }

    public IActionResult Index()
    {
        return View(_pinService.AccessiblePins(Request));
    }

    [HttpPost]
    public IActionResult SetKey(string key)
    {
        this.Response.Cookies.Append("key", key);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
