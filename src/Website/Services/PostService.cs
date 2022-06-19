using Website.Models;
using Website.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Website.Services.Mappers;

namespace Website.Services;

public class PostService : IPostService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IPostRepository _postRepository;
    private readonly IPostMapper _postMapper;

    public PostService(
        IMemoryCache memoryCache,
        IPostRepository postRepository,
        IPostMapper postMapper)
    {
        _memoryCache = memoryCache;
        _postRepository = postRepository;
        _postMapper = postMapper;
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
                    return _postMapper.MapPostData(postData);
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
            var postData = await _postRepository.GetPostDataAsync(post.Name);
            yield return _postMapper.MapPostTeaserData(post.Name, postData);
        }
    }
}