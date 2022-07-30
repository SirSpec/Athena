using Athena.Website.Models;
using Microsoft.Extensions.Caching.Memory;
using Athena.Website.Options;
using Microsoft.Extensions.Options;

namespace Athena.Website.Services;

public class CachedPostViewModelService : IPostViewModelService
{
    private readonly CacheOptions _cacheOptions;
    private readonly IMemoryCache _memoryCache;
    private readonly IPostViewModelService _postService;

    public CachedPostViewModelService(
        IOptionsSnapshot<CacheOptions> cacheOptions,
        IMemoryCache memoryCache,
        IPostViewModelService postService)
    {
        _cacheOptions = cacheOptions.Value;
        _memoryCache = memoryCache;
        _postService = postService;
    }

    public async Task<IEnumerable<PostTeaserViewModel>> GetPostTeaserViewModelsAsync() =>
        await _memoryCache.GetOrCreateAsync<IEnumerable<PostTeaserViewModel>>(
            "HomePage",
            async cacheEntry =>
            {
                var teasers = await _postService.GetPostTeaserViewModelsAsync();
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_cacheOptions.PostDataTimeToLiveInHours);
                return teasers;
            });

    public async Task<PostViewModel> GetPostViewModelAsync(string name) =>
        await _memoryCache.GetOrCreateAsync<PostViewModel>(
            name,
            async cacheEntry =>
            {
                var model = await _postService.GetPostViewModelAsync(name);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_cacheOptions.PostDataTimeToLiveInHours);
                return model;
            });
}