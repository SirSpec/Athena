using Athena.Domain.ValueObjects;
using Xunit;

namespace Athena.DomainTest;

public class PostNameTest
{
    [Theory]
    [InlineData("")]
    [InlineData("2test")]
    public void IsValidFormat_InvalidValue_False(string name)
    {
        var result = PostName.IsValidFormat(name);

        Assert.False(result);
    }

    [Theory]
    [InlineData("test")]
    [InlineData("test-name")]
    [InlineData("test-name1")]
    public void IsValidFormat_ValidValue_True(string name)
    {
        var result = PostName.IsValidFormat(name);

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