namespace Smart.Maui.Data;

using System.Globalization;

public sealed class ReverseConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void TrueReturnsFalse()
    {
        // Arrange
        var converter = new ReverseConverter();

        // Act
        var result = converter.Convert(true, typeof(bool), null, Culture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void FalseReturnsTrue()
    {
        // Arrange
        var converter = new ReverseConverter();

        // Act
        var result = converter.Convert(false, typeof(bool), null, Culture);

        // Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void NonBoolReturnsOriginal()
    {
        // Arrange
        var converter = new ReverseConverter();

        // Act
        var result = converter.Convert("hello", typeof(string), null, Culture);

        // Assert
        Assert.Equal("hello", result);
    }

    [Fact]
    public void ConvertBackTrueReturnsFalse()
    {
        // Arrange
        var converter = new ReverseConverter();

        // Act
        var result = converter.ConvertBack(true, typeof(bool), null, Culture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void ConvertBackFalseReturnsTrue()
    {
        // Arrange
        var converter = new ReverseConverter();

        // Act
        var result = converter.ConvertBack(false, typeof(bool), null, Culture);

        // Assert
        Assert.Equal(true, result);
    }
}
