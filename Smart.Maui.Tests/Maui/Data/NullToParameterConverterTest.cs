namespace Smart.Maui.Data;

using System.Globalization;

public sealed class NullToParameterConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void NullValueReturnsParameter()
    {
        // Arrange
        var converter = new NullToParameterConverter();

        // Act
        var result = converter.Convert(null, typeof(string), "default", Culture);

        // Assert
        Assert.Equal("default", result);
    }

    [Fact]
    public void NonNullValueReturnsValue()
    {
        // Arrange
        var converter = new NullToParameterConverter();

        // Act
        var result = converter.Convert("hello", typeof(string), "default", Culture);

        // Assert
        Assert.Equal("hello", result);
    }

    [Fact]
    public void InvertNullValueReturnsValue()
    {
        // Arrange
        var converter = new NullToParameterConverter { Invert = true };

        // Act
        // value is null: Invert means return value(null), not parameter
        var result = converter.Convert(null, typeof(string), "default", Culture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void InvertNonNullValueReturnsParameter()
    {
        // Arrange
        var converter = new NullToParameterConverter { Invert = true };

        // Act
        var result = converter.Convert("hello", typeof(string), "default", Culture);

        // Assert
        Assert.Equal("default", result);
    }

    [Fact]
    public void HandleEmptyStringTrueEmptyStringReturnsParameter()
    {
        // Arrange
        var converter = new NullToParameterConverter { HandleEmptyString = true };

        // Act
        var result = converter.Convert(string.Empty, typeof(string), "default", Culture);

        // Assert
        Assert.Equal("default", result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new NullToParameterConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("x", typeof(string), null, Culture));
    }
}
