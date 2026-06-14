namespace Smart.Maui.Data;

using System.Globalization;

public sealed class ColorToBrushConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ColorReturnsSolidColorBrush()
    {
        // Arrange
        var converter = new ColorToBrushConverter();

        // Act
        var result = converter.Convert(Colors.Red, typeof(SolidColorBrush), null, Culture);

        // Assert
        var brush = Assert.IsType<SolidColorBrush>(result);
        Assert.Equal(Colors.Red, brush.Color);
    }

    [Fact]
    public void NonColorReturnsNull()
    {
        // Arrange
        var converter = new ColorToBrushConverter();

        // Act
        var result = converter.Convert("red", typeof(SolidColorBrush), null, Culture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertBackBrushReturnsColor()
    {
        // Arrange
        var converter = new ColorToBrushConverter();
        var brush = new SolidColorBrush(Colors.Blue);

        // Act
        var result = converter.ConvertBack(brush, typeof(Color), null, Culture);

        // Assert
        Assert.Equal(Colors.Blue, result);
    }

    [Fact]
    public void ConvertBackNonBrushReturnsNull()
    {
        // Arrange
        var converter = new ColorToBrushConverter();

        // Act
        var result = converter.ConvertBack("not a brush", typeof(Color), null, Culture);

        // Assert
        Assert.Null(result);
    }
}
