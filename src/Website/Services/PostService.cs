using Website.Domain.ValueObjects;
using Website.Models;
using Website.Repositories;
using Website.Services.Mappers;

namespace Website.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IPostMapper _postMapper;

    public PostService(
        IPostRepository postRepository,
        IPostMapper postMapper)
    {
        _postRepository = postRepository;
        _postMapper = postMapper;
    }

    public async Task<PostViewModel> GetPostViewModelAsync(string name)
    {
        var postData = await _postRepository.GetPostAsync(new PostName(name));
        return _postMapper.MapPostData(postData);
    }

    public async Task<HomeViewModel> GetHomeViewModelAsync() =>
        new HomeViewModel { PostTeasers = await GetPostTeasersAsync().ToListAsync() };

    private async IAsyncEnumerable<PostTeaserViewModel> GetPostTeasersAsync()
    {
        var postNames = await _postRepository.GetPostNamesAsync();

        foreach (var postName in postNames)
        {
            var post = await _postRepository.GetPostAsync(postName);
            yield return _postMapper.MapPostTeaserData(post);
        }
    }
}