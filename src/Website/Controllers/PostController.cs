using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Services;
using Website.Services.Validators;

namespace Website.Controllers;

[Route("{controller}")]
public class PostController : Controller
{
    private readonly IPostService _postService;

    public PostController(IPostService postService) =>
        _postService = postService;

    [Route("{name}")]
    public async Task<IActionResult> Index([RegularExpression(PostValidator.PostNameRegexPattern)] string name) =>
        ModelState.IsValid &&
        await _postService.GetPostViewModelAsync(name) is PostViewModel viewModel &&
        viewModel != PostViewModel.Empty
            ? View(viewModel)
            : RedirectPreserveMethod("/error/404");
}