using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Services;

namespace Website.Controllers;

public class HomeController : Controller
{
    private readonly IPostService _postService;

    public HomeController(IPostService postService) =>
        _postService = postService;

    public async Task<IActionResult> Index()
    {
        var viewModel = await _postService.GetPostTeasersViewModelAsync();

        return viewModel.Any()
            ? View(new HomeViewModel { PostTeasers = viewModel })
            : RedirectPreserveMethod("/error/404");
    }
}