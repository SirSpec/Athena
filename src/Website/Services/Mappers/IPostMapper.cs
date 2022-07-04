using Website.Domain.ValueObjects;
using Website.Models;

namespace Website.Services.Mappers;

public interface IPostMapper
{
    PostViewModel MapPostData(Post post);
    PostTeaserViewModel MapPostTeaserData(Post post);
}