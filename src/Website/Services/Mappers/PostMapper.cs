using Website.Constants;
using Website.Extensions;
using Website.Models;
using Website.Services.Interpreters;

namespace Website.Services.Mappers;

public class PostMapper : IPostMapper
{
    private readonly IPostInterpreter _postInterpreter;

    public PostMapper(IPostInterpreter postInterpreter) =>
        _postInterpreter = postInterpreter;

    public PostViewModel MapPostData(string postData) =>
        new PostViewModel
        {
            PublishingDate = _postInterpreter.Interpret(postData, Tokens.PublishingDate).ToHtmlString(),
            Title = _postInterpreter.Interpret(postData, Tokens.Title).ToHtmlString(),
            Body = _postInterpreter.Interpret(postData, Tokens.Body).ToHtmlString()
        };

    public PostTeaserViewModel MapPostTeaserData(string postName, string postData) =>
        new PostTeaserViewModel
        {
            Name = postName.ToHtmlString(),
            PublishingDate = _postInterpreter.Interpret(postData, Tokens.PublishingDate).ToHtmlString(),
            Title = _postInterpreter.Interpret(postData, Tokens.Title).ToHtmlString(),
            Description = _postInterpreter.Interpret(postData, Tokens.Description).ToHtmlString()
        };
}