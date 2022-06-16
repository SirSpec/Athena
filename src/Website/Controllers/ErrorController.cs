using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Website.Models;

namespace Website.Controllers;

[Route("{controller}")]
public class ErrorController : Controller
{
    [Route("{code}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index(string code)
    {
        return View(new ErrorViewModel
        {
            ErrorCode = code,
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}