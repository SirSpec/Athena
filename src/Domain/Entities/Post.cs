using Athena.Domain.ValueObjects;

namespace Athena.Domain.Entities;

public record Post
{
    private readonly PostName _postName;
    private readonly PostBody _postBody;

    public Post(PostName postName, PostBody postBody)
    {
        _postName = postName;
        _postBody = postBody;
    }

    public string Id => _postName.Value;
    public string PublishingDate => _postBody.PublishingDate;
    public string Title => _postBody.Title;
    public string Description => _postBody.Description;
    public string Body => _postBody.Body;
}