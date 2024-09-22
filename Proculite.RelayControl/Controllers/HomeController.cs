using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proculite.RelayControl.Models;

namespace Proculite.RelayControl.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;
    private readonly KeyPinControl[] _keyPinControl;
    private readonly Dictionary<string, int[]> _pinControlMap;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _keyPinControl = configuration.GetSection("KeyPinControl")
            .GetChildren()
            .Select(keyPinControl => new KeyPinControl{
                Key = keyPinControl.GetSection("Key").Get<string>() ?? "",
                Pins = keyPinControl.GetSection("Pins").GetChildren().Select(pin => pin.Get<int>()).ToArray()
            })
            .ToArray();
        _pinControlMap = _keyPinControl.ToDictionary(pinControl => pinControl.Key, pinControl => pinControl.Pins);
    }

    public IActionResult Index()
    {
        return View();
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
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
