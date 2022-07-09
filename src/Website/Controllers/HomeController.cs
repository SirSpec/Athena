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
        var postTeasers = await _postService.GetPostTeaserViewModelsAsync();

        return postTeasers.Any()
            ? View(postTeasers)
            : RedirectPreserveMethod($"/error/{((int)HttpStatusCode.NotFound)}");
    }
}