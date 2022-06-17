using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Services;

namespace Website.Controllers;

[Route("{controller}")]
public class PostController : Controller
{
    private readonly IPostService _postService;

    public PostController(IPostService postService) =>
        _postService = postService;

    [Route("{name}")]
    public async Task<IActionResult> Index([Required] string name)
    {
        var viewModel = await _postService.GetPostViewModelAsync(name);

        return viewModel != PostViewModel.Empty
            ? View(viewModel)
            : RedirectPreserveMethod("/error/404");
    }
}