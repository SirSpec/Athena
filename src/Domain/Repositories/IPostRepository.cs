using Athena.Domain.Entities;
using Athena.Domain.ValueObjects;

namespace Athena.Domain.Repositories;

public interface IPostRepository
{
    Task<IEnumerable<PostName>> GetPostNamesAsync();
    Task<Post> GetPostAsync(PostName postName);
}