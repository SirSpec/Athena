using Website.Extensions;
using Website.Models;
using Website.Constants;
using Website.Repositories;
using Website.Services.Interpreters;
using Microsoft.Extensions.Caching.Memory;

namespace Website.Services;

public class PostService : IPostService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IPostRepository _postRepository;
    private readonly IPostInterpreter _postInterpreter;

    public PostService(
        IMemoryCache memoryCache,
        IPostRepository postRepository,
        IPostInterpreter postInterpreter)
    {
        _memoryCache = memoryCache;
        _postRepository = postRepository;
        _postInterpreter = postInterpreter;
    }

    public async Task<IEnumerable<PostTeaserViewModel>> GetPostTeasersViewModelAsync() =>
        await _memoryCache.GetOrCreateAsync<IEnumerable<PostTeaserViewModel>>(
            "HomePage",
            async cacheEntry =>
            {
                var teasers = await GetPostTeasersAsync().ToListAsync();

                cacheEntry.AbsoluteExpirationRelativeToNow = teasers.Any()
                    ? TimeSpan.FromDays(1)
                    : TimeSpan.FromSeconds(30);

                return teasers;
            });

    public async Task<PostViewModel> GetPostViewModelAsync(string name) =>
        await _memoryCache.GetOrCreateAsync<PostViewModel>(
            name,
            async cacheEntry =>
            {
                var postData = await _postRepository.GetPostDataAsync(name);

                if (string.IsNullOrWhiteSpace(postData) is false)
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
                    return new PostViewModel
                    {
                        PublishingDate = _postInterpreter.Interpret(postData, Tokens.PublishingDate).ToHtmlString(),
                        Title = _postInterpreter.Interpret(postData, Tokens.Title).ToHtmlString(),
                        Body = _postInterpreter.Interpret(postData, Tokens.Body).ToHtmlString()
                    };
                }
                else
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                    return PostViewModel.Empty;
                }
            });

    private async IAsyncEnumerable<PostTeaserViewModel> GetPostTeasersAsync()
    {
        var posts = await _postRepository.GetPostTeasersAsync();

        foreach (var post in posts)
        {
            var postContent = await _postRepository.GetPostDataAsync(post.Name);

            yield return new PostTeaserViewModel
            {
                Name = post.Name.ToHtmlString(),
                PublishingDate = _postInterpreter.Interpret(postContent, Tokens.PublishingDate).ToHtmlString(),
                Title = _postInterpreter.Interpret(postContent, Tokens.Title).ToHtmlString(),
                Description = _postInterpreter.Interpret(postContent, Tokens.Description).ToHtmlString()
            };
        }
    }
}