using System.Net;
using Microsoft.AspNetCore.Mvc;
using Website.Services;

namespace Website.Controllers;

public class HomeController : Controller
{
    private readonly IPostService _postService;

    public HomeController(IPostService postService) =>
        _postService = postService;

    public async Task<IActionResult> Index()
    {
        var viewModel = await _postService.GetHomeViewModelAsync();

        return viewModel.PostTeasers.Any()
            ? View(viewModel)
            : RedirectPreserveMethod($"/error/{((int)HttpStatusCode.NotFound)}");
    }
}