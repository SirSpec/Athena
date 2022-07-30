using System.ComponentModel.DataAnnotations;
using System.Net;
using Athena.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Athena.Website.Models;
using Athena.Website.Services;

namespace Athena.Website.Controllers;

[Route("{controller}")]
public class PostController : Controller
{
    private readonly IPostViewModelService _postService;

    public PostController(IPostViewModelService postService) =>
        _postService = postService;

    [Route("{name}")]
    public async Task<IActionResult> Index([RegularExpression(PostName.NameFormatRegexPattern)] string name) =>
        ModelState.IsValid && await _postService.GetPostViewModelAsync(name) is PostViewModel viewModel
            ? View(viewModel)
            : RedirectPreserveMethod($"/error/{((int)HttpStatusCode.BadRequest)}");
}