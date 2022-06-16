using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Services;

namespace Website.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPostService _postService;

    public HomeController(ILogger<HomeController> logger, IPostService postService)
    {
        _logger = logger;
        _postService = postService;
    }

    public async Task<IActionResult> Index()
    {
        var postTeasers = _postService.GetPostTeasersViewModelAsync();

        return View(new HomeViewModel
        {
            PostTeasers = await postTeasers.ToListAsync()
        });
    }
}