using Website.Models;

namespace Website.Services.Mappers;

public interface IPostMapper
{
    PostViewModel MapPostData(string postData);
    PostTeaserViewModel MapPostTeaserData(string postName, string postData);
}