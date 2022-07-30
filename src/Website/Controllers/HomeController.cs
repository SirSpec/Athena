using System.Net;
using Microsoft.AspNetCore.Mvc;
using Athena.Website.Services;

namespace Athena.Website.Controllers;

public class HomeController : Controller
{
    private readonly IPostViewModelService _postService;

    public HomeController(IPostViewModelService postService) =>
        _postService = postService;

    public async Task<IActionResult> Index()
    {
        var postTeasers = await _postService.GetPostTeaserViewModelsAsync();

        return postTeasers.Any()
            ? View(postTeasers)
            : RedirectPreserveMethod($"/error/{((int)HttpStatusCode.NotFound)}");
    }
}