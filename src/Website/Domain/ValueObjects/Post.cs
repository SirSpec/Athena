using Website.Domain.Constants;
using Website.Domain.Interpreters;

namespace Website.Domain.ValueObjects;

public record Post
{
    private readonly PostName _postName;

    public Post(PostName postName, string postData)
    {
        _postName = postName;

        var _postInterpreter = new PostInterpreter();
        PublishingDate = _postInterpreter.Interpret(postData, Tokens.PublishingDate);
        Title = _postInterpreter.Interpret(postData, Tokens.Title);
        Description = _postInterpreter.Interpret(postData, Tokens.Description);
        Body = _postInterpreter.Interpret(postData, Tokens.Body);
    }

    public string Name => _postName.Value;
    public string PublishingDate { get; }
    public string Title { get; }
    public string Description { get; }
    public string Body { get; }
}