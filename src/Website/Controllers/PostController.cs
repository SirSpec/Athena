using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Website.Services;

namespace Website.Controllers;

[Route("{controller}")]
public class PostController : Controller
{
    private readonly ILogger<PostController> _logger;
    private readonly IPostService _postService;

    public PostController(ILogger<PostController> logger, IPostService postService)
    {
        _logger = logger;
        _postService = postService;
    }

    [Route("{name}")]
    public async Task<IActionResult> Index([Required] string name)
    {
        var post = await _postService.GetPostViewModelAsync(name);
        return View(post);
    }
}