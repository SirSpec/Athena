using Athena.Domain.Repositories;
using Athena.Domain.ValueObjects;
using Athena.Website.Models;
using Athena.Website.Services.Mappers;

namespace Athena.Website.Services;

public class PostViewModelService : IPostViewModelService
{
    private readonly IPostRepository _postRepository;
    private readonly IPostMapper _postMapper;

    public PostViewModelService(
        IPostRepository postRepository,
        IPostMapper postMapper)
    {
        _postRepository = postRepository;
        _postMapper = postMapper;
    }

    public async Task<PostViewModel> GetPostViewModelAsync(string name)
    {
        var post = await _postRepository.GetPostAsync(new PostName(name));
        return _postMapper.MapPostData(post);
    }

    public async Task<IEnumerable<PostTeaserViewModel>> GetPostTeaserViewModelsAsync() =>
        await GetPostTeasersAsync().ToListAsync();

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