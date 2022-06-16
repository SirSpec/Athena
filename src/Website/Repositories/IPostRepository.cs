using Website.Repositories.Models;

namespace Website.Repositories;

public interface IPostRepository
{
    Task<IEnumerable<ContentItem>> GetPostTeasersAsync();
    Task<string> GetPostDataAsync(string url);
}