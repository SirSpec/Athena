using Microsoft.Extensions.Options;
using Website.Options;
using Website.Repositories.Models;

namespace Website.Repositories;

public class PostRepository : IPostRepository
{
    private readonly ILogger<PostRepository> _logger;
    private readonly ApiOptions _apiOptions;
    private readonly HttpClient _httpClient;

    public PostRepository(
        ILogger<PostRepository> logger,
        IOptions<ApiOptions> apiOptions,
        HttpClient httpClient)
    {
        _logger = logger;
        _apiOptions = apiOptions.Value;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ContentItem>> GetPostTeasersAsync() =>
        await _httpClient.GetFromJsonAsync<IEnumerable<ContentItem>>(
                new Uri(_apiOptions.PostApiUrl))
            ?? Enumerable.Empty<ContentItem>();

    public async Task<string> GetPostDataAsync(string path) =>
        await _httpClient.GetStringAsync(new Uri($"{_apiOptions.RawDataApiUrl}{path}"));
}