using Microsoft.AspNetCore.Mvc;
using Athena.Website.Models;

namespace Athena.Website.Controllers;

[Route("{controller}")]
public class ErrorController : Controller
{
    [Route("{code}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index(string code) =>
        View(new ErrorViewModel { ErrorCode = code });
}