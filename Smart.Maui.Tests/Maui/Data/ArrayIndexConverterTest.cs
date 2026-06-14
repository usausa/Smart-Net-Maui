namespace Smart.Maui.Data;

using System.Globalization;

public sealed class ArrayIndexConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    private static readonly string[] SingleElement = ["a"];

    [Fact]
    public void ConvertReturnsElementAtIndex()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new[] { "a", "b", "c" };

        // Act
        var result = converter.Convert(1, typeof(string), array, Culture);

        // Assert
        Assert.Equal("b", result);
    }

    [Fact]
    public void ConvertNullValueReturnsNull()
    {
        // Arrange
        var converter = new ArrayIndexConverter();

        // Act
        var result = converter.Convert(null, typeof(string), SingleElement, Culture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertNullParameterReturnsNull()
    {
        // Arrange
        var converter = new ArrayIndexConverter();

        // Act
        var result = converter.Convert(0, typeof(string), null, Culture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertBackReturnsIndex()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new[] { "a", "b", "c" };

        // Act
        var result = converter.ConvertBack("b", typeof(int), array, Culture);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void ConvertBackNotFoundReturnsMinusOne()
    {
        // Arrange
        var converter = new ArrayIndexConverter();
        var array = new[] { "a", "b" };

        // Act
        var result = converter.ConvertBack("z", typeof(int), array, Culture);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void ConvertBackNullParameterReturnsMinusOne()
    {
        // Arrange
        var converter = new ArrayIndexConverter();

        // Act
        var result = converter.ConvertBack("x", typeof(int), null, Culture);

        // Assert
        Assert.Equal(-1, result);
    }
}
