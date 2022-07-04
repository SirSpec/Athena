using Website.Domain.ValueObjects;
using Website.Extensions;
using Website.Models;

namespace Website.Services.Mappers;

public class PostMapper : IPostMapper
{
    public PostViewModel MapPostData(Post post) =>
        new PostViewModel
        {
            PublishingDate = post.PublishingDate.ToHtmlString(),
            Title = post.Title.ToHtmlString(),
            Body = post.Body.ToHtmlString()
        };

    public PostTeaserViewModel MapPostTeaserData(Post post) =>
        new PostTeaserViewModel
        {
            Name = post.Name.ToHtmlString(),
            PublishingDate = post.PublishingDate.ToHtmlString(),
            Title = post.Title.ToHtmlString(),
            Description = post.Description.ToHtmlString()
        };
}