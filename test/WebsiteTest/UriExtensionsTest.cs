using Xunit;

namespace Athena.WebsiteTest;

public class UriExtensionsTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("/relative/url")]
    public void ToAbsoluteUri_InvalidUrl_ArgumentException(string url)
    {
        Action action = () => url.ToAbsoluteUri();

        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void ToAbsoluteUri_AbsoluteUrl_AbsoluteUri()
    {
        var sut = "https://absolute.url/test";

        var result = sut.ToAbsoluteUri();

        Assert.True(result.IsAbsoluteUri);
    }
}