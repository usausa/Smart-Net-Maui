namespace Smart.Maui.Data;

using System.Globalization;

public sealed class ColorBlendConverterTests
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void BlendAtZeroReturnsSameColor()
    {
        // Arrange
        var converter = new ColorBlendConverter
        {
            Color = new Color(1f, 0f, 0f), // red
            Raito = 0d
        };
        var source = new Color(0.2f, 0.4f, 0.6f);

        // Act
        var result = converter.Convert(source, typeof(Color), null, Culture);

        // Assert
        var color = Assert.IsType<Color>(result);
        Assert.Equal(0.2f, color.Red);
        Assert.Equal(0.4f, color.Green);
        Assert.Equal(0.6f, color.Blue);
    }

    [Fact]
    public void BlendAtOneReturnsTargetColor()
    {
        // Arrange
        var target = new Color(0.2f, 0.4f, 0.6f);
        var converter = new ColorBlendConverter { Color = target, Raito = 1d };

        // Act
        var result = converter.Convert(new Color(1f, 0f, 0f), typeof(Color), null, Culture);

        // Assert
        var color = Assert.IsType<Color>(result);
        Assert.Equal(0.2f, color.Red);
        Assert.Equal(0.4f, color.Green);
        Assert.Equal(0.6f, color.Blue);
    }

    [Fact]
    public void BlendAtHalfProducesIntermediateColor()
    {
        // Arrange
        // Color components are 0..1 floats; blending black and red at 0.5 gives Red = 0.5
        var source = new Color(0f, 0f, 0f);
        var target = new Color(1f, 0f, 0f);
        var converter = new ColorBlendConverter { Color = target, Raito = 0.5d };

        // Act
        var result = converter.Convert(source, typeof(Color), null, Culture);

        // Assert
        var color = Assert.IsType<Color>(result);
        Assert.Equal(0.5f, color.Red);
        Assert.Equal(0f, color.Green);
        Assert.Equal(0f, color.Blue);
        Assert.Equal(1f, color.Alpha);
    }

    [Fact]
    public void NonColorInputReturnsNull()
    {
        // Arrange
        var converter = new ColorBlendConverter { Color = Colors.Red, Raito = 0.5d };

        // Act
        var result = converter.Convert("not a color", typeof(Color), null, Culture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new ColorBlendConverter { Color = Colors.Red, Raito = 0.5d };

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack(Colors.Red, typeof(Color), null, Culture));
    }

    [Fact]
    public void InvalidRaitoThrowsArgumentOutOfRange()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ColorBlendConverter { Raito = 1.5d });
    }
}
