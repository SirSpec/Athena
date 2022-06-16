using Website.Models;

namespace Website.Services;

public interface IPostService
{
    IAsyncEnumerable<PostTeaserViewModel> GetPostTeasersViewModelAsync();
    Task<PostViewModel> GetPostViewModelAsync(string url);
}