using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Options;
using Website.Models;
using Website.Options;

namespace Website.Services;

public class PostService : IPostService
{
    private readonly HttpClient _httpClient;
    private readonly ApiOptions _apiOptions;

    public PostService(HttpClient httpClient, IOptions<ApiOptions> apiOptions)
    {
        _httpClient = httpClient;
        _apiOptions = apiOptions.Value;
    }

    public async IAsyncEnumerable<PostTeaserViewModel> GetPostTeasers()
    {
        var posts = await _httpClient.GetFromJsonAsync<IEnumerable<ContentItem>>(_apiOptions.ContentPath)
            ?? Enumerable.Empty<ContentItem>();

        foreach (var item in posts)
        {
            var postContent = await _httpClient.GetStringAsync(item.DownloadUrl);
            yield return new PostTeaserViewModel
            {
                Id = item.Sha ?? string.Empty,
                PublishingDate = new HtmlString(DateTime.Now.ToShortDateString()),
                Title = new HtmlString(item.Name),
                Description = new HtmlString(postContent),
                Url = new HtmlString(item.DownloadUrl)
            };
        }
    }

    public async Task<PostViewModel> GetPost(string url)
    {
        var postContent = await _httpClient.GetStringAsync(url);

        return new PostViewModel
        {
            PublishingDate = new HtmlString(DateTime.Now.ToShortDateString()),
            Title = new HtmlString(url),
            Description = new HtmlString(postContent),
        };
    }
}
