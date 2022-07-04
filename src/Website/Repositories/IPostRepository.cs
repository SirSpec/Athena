using Website.Domain.ValueObjects;

namespace Website.Repositories;

public interface IPostRepository
{
    Task<IEnumerable<PostName>> GetPostNamesAsync();
    Task<Post> GetPostAsync(PostName postName);
}