namespace Website.Models;

public class HomeViewModel
{
    public IEnumerable<PostTeaserViewModel> PostTeasers { get; init; } = Enumerable.Empty<PostTeaserViewModel>();
}
