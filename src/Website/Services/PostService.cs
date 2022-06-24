using Website.Models;
using Website.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Website.Services.Mappers;
using Website.Options;
using Microsoft.Extensions.Options;

namespace Website.Services;

public class PostService : IPostService
{
    private readonly CacheOptions _cacheOptions;
    private readonly IMemoryCache _memoryCache;
    private readonly IPostRepository _postRepository;
    private readonly IPostMapper _postMapper;

    public PostService(
        IOptions<CacheOptions> cacheOptions,
        IMemoryCache memoryCache,
        IPostRepository postRepository,
        IPostMapper postMapper)
    {
        _cacheOptions = cacheOptions.Value;
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
                    ? TimeSpan.FromDays(_cacheOptions.AbsoluteExpirationRelativeToNowOk)
                    : TimeSpan.FromSeconds(_cacheOptions.AbsoluteExpirationRelativeToNowNotFound);

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
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(_cacheOptions.AbsoluteExpirationRelativeToNowOk);
                    return _postMapper.MapPostData(postData);
                }
                else
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheOptions.AbsoluteExpirationRelativeToNowNotFound);
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