using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Website.Domain.ValueObjects;
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
    public async Task<IActionResult> Index([RegularExpression(PostName.ValueRegexPattern)] string name) =>
        ModelState.IsValid && await _postService.GetPostViewModelAsync(name) is PostViewModel viewModel
            ? View(viewModel)
            : RedirectPreserveMethod($"/error/{((int)HttpStatusCode.BadRequest)}");
}