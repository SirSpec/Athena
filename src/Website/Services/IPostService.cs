using Website.Models;

namespace Website.Services;

public interface IPostService
{
    Task<IEnumerable<PostTeaserViewModel>> GetPostTeaserViewModelsAsync();
    Task<PostViewModel> GetPostViewModelAsync(string url);
}