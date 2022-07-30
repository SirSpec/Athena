using Athena.Domain.Entities;
using Athena.Website.Models;

namespace Athena.Website.Services.Mappers;

public interface IPostMapper
{
    PostViewModel MapPostData(Post post);
    PostTeaserViewModel MapPostTeaserData(Post post);
}