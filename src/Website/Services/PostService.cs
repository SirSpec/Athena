using Microsoft.AspNetCore.Html;
using Website.Extensions;
using Website.Models;
using Website.Constants;
using Website.Repositories;
using Website.Services.Interpreters;

namespace Website.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IPostInterpreter _postInterpreter;

    public PostService(
        IPostRepository postRepository,
        IPostInterpreter postInterpreter)
    {
        _postRepository = postRepository;
        _postInterpreter = postInterpreter;
    }

    public async IAsyncEnumerable<PostTeaserViewModel> GetPostTeasersViewModelAsync()
    {
        var posts = await _postRepository.GetPostTeasersAsync();

        foreach (var post in posts)
        {
            var postContent = await _postRepository.GetPostDataAsync(post.Name ?? string.Empty);

            yield return new PostTeaserViewModel
            {
                Name = new HtmlString(post.Name),
                PublishingDate = _postInterpreter.Interpret(postContent, Tokens.PublishingDate).ToHtmlString(),
                Title = _postInterpreter.Interpret(postContent, Tokens.Title).ToHtmlString(),
                Description = _postInterpreter.Interpret(postContent, Tokens.Description).ToHtmlString()
            };
        }
    }

    public async Task<PostViewModel> GetPostViewModelAsync(string name)
    {
        var postData = await _postRepository.GetPostDataAsync(name);

        return new PostViewModel
        {
            PublishingDate = _postInterpreter.Interpret(postData, Tokens.PublishingDate).ToHtmlString(),
            Title = _postInterpreter.Interpret(postData, Tokens.Title).ToHtmlString(),
            Body = _postInterpreter.Interpret(postData, Tokens.Body).ToHtmlString()
        };
    }
}