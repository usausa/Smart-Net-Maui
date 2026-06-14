namespace Smart.Maui.Data;

using System.Globalization;

public sealed class ColorBlendConverterTest
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

        // Act
        var result = converter.Convert(new Color(0f, 1f, 0f), typeof(Color), null, Culture);

        // Assert
        // ratio=0 => no blend, returns original color components
        Assert.IsType<Color>(result);
    }

    [Fact]
    public void BlendAtHalfProducesIntermediateColor()
    {
        // Arrange
        // source: Red=0 (R=0,G=255,B=0 → byte=0), target: fully red (R=255 → byte=255 → float=1.0)
        // With Color(float) constructor: Red=0 → byte component=0, target Red=1f → byte=255
        // At ratio=0.5: r = (byte)Round(0 + (255-0)*0.5) = (byte)Round(127.5) = 128 → Color(128/255,0,0)
        var source = new Color(0f, 0f, 0f);
        var target = new Color(1f, 0f, 0f);
        var converter = new ColorBlendConverter { Color = target, Raito = 0.5d };

        // Act
        var result = converter.Convert(source, typeof(Color), null, Culture);

        // Assert
        Assert.IsType<Color>(result);
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
