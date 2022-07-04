using Website.Models;

namespace Website.Services;

public interface IPostService
{
    Task<HomeViewModel> GetHomeViewModelAsync();
    Task<PostViewModel> GetPostViewModelAsync(string url);
}