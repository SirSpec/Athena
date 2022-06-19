using Microsoft.Extensions.Options;
using Website.Options;
using Website.Repositories.Models;
using Website.Services.Validators;

namespace Website.Repositories;

public class PostRepository : IPostRepository
{
    private readonly ILogger<PostRepository> _logger;
    private readonly ApiOptions _apiOptions;
    private readonly HttpClient _httpClient;
    private readonly IPostValidator _postValidator;

    public PostRepository(
        ILogger<PostRepository> logger,
        IOptions<ApiOptions> apiOptions,
        HttpClient httpClient,
        IPostValidator postValidator)
    {
        _logger = logger;
        _apiOptions = apiOptions.Value;
        _httpClient = httpClient;
        _postValidator = postValidator;
    }

    public async Task<IEnumerable<ContentItem>> GetPostTeasersAsync()
    {
        try
        {
            return Uri.TryCreate(_apiOptions.PostApiUrl, UriKind.RelativeOrAbsolute, out var uri)
                ? (await _httpClient.GetFromJsonAsync<IEnumerable<ContentItem>>(uri) ?? Enumerable.Empty<ContentItem>())
                    .Where(item => _postValidator.IsNameValid(item.Name))
                : Enumerable.Empty<ContentItem>();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not get posts.");
            return Enumerable.Empty<ContentItem>();
        }
    }

    public async Task<string> GetPostDataAsync(string name)
    {
        try
        {
            return _postValidator.IsNameValid(name) && Uri.TryCreate($"{_apiOptions.RawDataApiUrl}{name}", UriKind.RelativeOrAbsolute, out var uri)
                ? await _httpClient.GetStringAsync(uri)
                : string.Empty;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not get post content.");
            return string.Empty;
        }
    }
}