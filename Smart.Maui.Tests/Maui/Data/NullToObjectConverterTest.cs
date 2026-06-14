namespace Smart.Maui.Data;

using System.Globalization;

public sealed class NullToObjectConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void NullInputReturnsNullValue()
    {
        // Arrange
        var converter = new NullToTextConverter { NullValue = "null", NonNullValue = "notnull" };

        // Act
        var result = converter.Convert(null, typeof(string), null, Culture);

        // Assert
        Assert.Equal("null", result);
    }

    [Fact]
    public void NonNullInputReturnsNonNullValue()
    {
        // Arrange
        var converter = new NullToTextConverter { NullValue = "null", NonNullValue = "notnull" };

        // Act
        var result = converter.Convert("hello", typeof(string), null, Culture);

        // Assert
        Assert.Equal("notnull", result);
    }

    [Fact]
    public void HandleEmptyStringTrueEmptyStringReturnsNullValue()
    {
        // Arrange
        var converter = new NullToTextConverter
        {
            NullValue = "empty",
            NonNullValue = "filled",
            HandleEmptyString = true
        };

        // Act
        var result = converter.Convert(string.Empty, typeof(string), null, Culture);

        // Assert
        Assert.Equal("empty", result);
    }

    [Fact]
    public void HandleEmptyStringFalseEmptyStringReturnsNonNullValue()
    {
        // Arrange
        var converter = new NullToTextConverter
        {
            NullValue = "empty",
            NonNullValue = "filled",
            HandleEmptyString = false
        };

        // Act
        var result = converter.Convert(string.Empty, typeof(string), null, Culture);

        // Assert
        Assert.Equal("filled", result);
    }

    [Fact]
    public void NullToBoolDefaultValues()
    {
        // Arrange
        var converter = new NullToBoolConverter();

        // Act & Assert
        Assert.Equal(false, converter.Convert(null, typeof(bool), null, Culture));
        Assert.Equal(true, converter.Convert("x", typeof(bool), null, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new NullToTextConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("null", typeof(string), null, Culture));
    }
}
