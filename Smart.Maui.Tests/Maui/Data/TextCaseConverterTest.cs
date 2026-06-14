namespace Smart.Maui.Data;

using System.Globalization;

public sealed class TextCaseConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ToUpperConvertsText()
    {
        // Arrange
        var converter = new ToUpperConverter();

        // Act
        var result = converter.Convert("hello", typeof(string), null, Culture);

        // Assert
        Assert.Equal("HELLO", result);
    }

    [Fact]
    public void ToUpperNullReturnsNull()
    {
        // Arrange
        var converter = new ToUpperConverter();

        // Act
        var result = converter.Convert(null, typeof(string), null, Culture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ToUpperNonStringReturnsNull()
    {
        // Arrange
        var converter = new ToUpperConverter();

        // Act
        var result = converter.Convert(42, typeof(string), null, Culture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ToUpperConvertBackThrows()
    {
        // Arrange
        var converter = new ToUpperConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("HELLO", typeof(string), null, Culture));
    }

    [Fact]
    public void ToLowerConvertsText()
    {
        // Arrange
        var converter = new ToLowerConverter();

        // Act
        var result = converter.Convert("HELLO", typeof(string), null, Culture);

        // Assert
        Assert.Equal("hello", result);
    }

    [Fact]
    public void ToLowerNullReturnsNull()
    {
        // Arrange
        var converter = new ToLowerConverter();

        // Act
        var result = converter.Convert(null, typeof(string), null, Culture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ToLowerConvertBackThrows()
    {
        // Arrange
        var converter = new ToLowerConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("hello", typeof(string), null, Culture));
    }
}
