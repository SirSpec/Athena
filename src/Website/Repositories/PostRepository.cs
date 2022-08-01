using Microsoft.Extensions.Options;
using Athena.Website.Options;
using Athena.Website.Repositories.Models;
using Athena.Domain.ValueObjects;
using Athena.Domain.Entities;
using Athena.Domain.Repositories;

namespace Athena.Website.Repositories;

public class PostRepository : IPostRepository
{
    private readonly ApiOptions _apiOptions;
    private readonly HttpClient _httpClient;

    public PostRepository(
        IOptions<ApiOptions> apiOptions,
        HttpClient httpClient)
    {
        _apiOptions = apiOptions.Value;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<PostName>> GetPostNamesAsync()
    {
        var uri = _apiOptions.PostApiUrl.ToAbsoluteUri();
        var contentItems = await _httpClient.GetFromJsonAsync<IEnumerable<ContentItem>>(uri);

        return contentItems?
            .Where(contentItem => PostName.IsValidFormat(contentItem.Name))
            .Select(contentItem => new PostName(contentItem.Name)) ?? Enumerable.Empty<PostName>();
    }

    public async Task<Post> GetPostAsync(PostName postName)
    {
        var uri = $"{_apiOptions.RawDataApiUrl}{postName.Value}".ToAbsoluteUri();
        var postData = await _httpClient.GetStringAsync(uri);

        return new Post(postName, new PostBody(postData));
    }
}