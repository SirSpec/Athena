using Website.Models;

namespace Website.Services;

public interface IPostService
{
    IAsyncEnumerable<PostTeaserViewModel> GetPostTeasers();
    Task<PostViewModel> GetPost(string url);
}