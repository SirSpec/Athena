using Website.Models;

namespace Website.Services;

public interface IPostService
{
    Task<IEnumerable<PostTeaserViewModel>> GetPostTeasersViewModelAsync();
    Task<PostViewModel> GetPostViewModelAsync(string url);
}