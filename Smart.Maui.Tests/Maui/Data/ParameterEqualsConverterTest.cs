namespace Smart.Maui.Data;

using System.Globalization;

public sealed class ParameterEqualsConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void EqualReturnsTrue()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act
        var result = converter.Convert("hello", typeof(bool), "hello", Culture);

        // Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void NotEqualReturnsFalse()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act
        var result = converter.Convert("hello", typeof(bool), "world", Culture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void BothNullReturnsTrue()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act
        var result = converter.Convert(null, typeof(bool), null, Culture);

        // Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void ConvertBackTrueReturnsParameter()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act
        var result = converter.ConvertBack(true, typeof(string), "tag", Culture);

        // Assert
        Assert.Equal("tag", result);
    }

    [Fact]
    public void ConvertBackFalseReturnsNull()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act
        var result = converter.ConvertBack(false, typeof(string), "tag", Culture);

        // Assert
        Assert.Null(result);
    }
}
