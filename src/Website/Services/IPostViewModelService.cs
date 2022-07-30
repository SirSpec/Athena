using Athena.Website.Models;

namespace Athena.Website.Services;

public interface IPostViewModelService
{
    Task<IEnumerable<PostTeaserViewModel>> GetPostTeaserViewModelsAsync();
    Task<PostViewModel> GetPostViewModelAsync(string url);
}