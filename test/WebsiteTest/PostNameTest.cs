using Xunit;
using Website.Domain.ValueObjects;

namespace WebsiteTest;

public class PostNameTest
{
    [Theory]
    [InlineData("")]
    [InlineData("2test")]
    public void IsNameValid_InvalidValue_False(string name)
    {
        var result = PostName.IsNameValid(name);

        Assert.False(result);
    }

    [Theory]
    [InlineData("test")]
    [InlineData("test-name")]
    [InlineData("test-name1")]
    public void IsNameValid_ValidValue_True(string name)
    {
        var result = PostName.IsNameValid(name);

        Assert.True(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("2test")]
    public void Constructor_Null_ArgumentException(string name)
    {
        Action action = () => new PostName(name);

        Assert.Throws<ArgumentException>(action);
    }
}