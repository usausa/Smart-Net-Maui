namespace Smart.Maui.Markup;

public sealed class ConverterExtensionTest
{
    private static readonly IServiceProvider NullServiceProvider = null!;

    [Fact]
    public void BoolToTextExtensionCreatesConverter()
    {
        // Arrange
        var ext = new BoolToTextExtension { True = "yes", False = "no" };

        // Act
        var converter = ext.ProvideValue(NullServiceProvider);

        // Assert
        Assert.Equal("yes", converter.TrueValue);
        Assert.Equal("no", converter.FalseValue);
    }

    [Fact]
    public void NullToBoolExtensionDefaultValues()
    {
        // Arrange
        var ext = new NullToBoolExtension();

        // Act
        var converter = ext.ProvideValue(NullServiceProvider);

        // Assert
        // Invert=false (default): NullValue=!false=true, NonNullValue=false
        Assert.True(converter.NullValue);
        Assert.False(converter.NonNullValue);
    }

    [Fact]
    public void NullToBoolExtensionInvert()
    {
        // Arrange
        var ext = new NullToBoolExtension { Invert = true };

        // Act
        var converter = ext.ProvideValue(NullServiceProvider);

        // Assert
        // Invert=true: NullValue=!true=false, NonNullValue=true
        Assert.False(converter.NullValue);
        Assert.True(converter.NonNullValue);
    }

    [Fact]
    public void NullToTextExtensionCreatesConverter()
    {
        // Arrange
        var ext = new NullToTextExtension { Null = "empty", NonNull = "filled" };

        // Act
        var converter = ext.ProvideValue(NullServiceProvider);

        // Assert
        Assert.Equal("empty", converter.NullValue);
        Assert.Equal("filled", converter.NonNullValue);
    }

    [Fact]
    public void TextToBoolExtensionCreatesConverter()
    {
        // Arrange
        var ext = new TextToBoolExtension { True = "on", False = "off" };

        // Act
        var converter = ext.ProvideValue(NullServiceProvider);

        // Assert
        Assert.Equal("on", converter.TrueValue);
        Assert.Equal("off", converter.FalseValue);
    }

    [Fact]
    public void IntToBoolExtensionCreatesConverter()
    {
        // Arrange
        var ext = new IntToBoolExtension { True = 1, False = 0 };

        // Act
        var converter = ext.ProvideValue(NullServiceProvider);

        // Assert
        Assert.Equal(1, converter.TrueValue);
        Assert.Equal(0, converter.FalseValue);
    }

    [Fact]
    public void CompareToBoolExtensionCreatesConverter()
    {
        // Arrange
        var ext = new CompareToBoolExtension();

        // Act
        var converter = ext.ProvideValue(NullServiceProvider);

        // Assert
        Assert.True(converter.TrueValue);
        Assert.False(converter.FalseValue);
        Assert.NotNull(converter.Expression);
    }

    [Fact]
    public void ContainsToBoolExtensionDefaultCreatesConverter()
    {
        // Arrange
        var ext = new ContainsToBoolExtension();

        // Act
        var converter = ext.ProvideValue(NullServiceProvider);

        // Assert
        // default: TrueValue=true, FalseValue=false
        Assert.True(converter.TrueValue);
        Assert.False(converter.FalseValue);
    }

    [Fact]
    public void ContainsToBoolExtensionInvert()
    {
        // Arrange
        var ext = new ContainsToBoolExtension { Invert = true };

        // Act
        var converter = ext.ProvideValue(NullServiceProvider);

        // Assert
        Assert.False(converter.TrueValue);
        Assert.True(converter.FalseValue);
    }

    [Fact]
    public void TextReplaceExtensionCreatesConverter()
    {
        // Arrange
        var ext = new TextReplaceExtension { Pattern = "x", Replacement = "y" };

        // Act
        var converter = ext.ProvideValue(NullServiceProvider);

        // Assert
        Assert.Equal("x", converter.Pattern);
        Assert.Equal("y", converter.Replacement);
    }

    [Fact]
    public void ColorBlendExtensionCreatesConverter()
    {
        // Arrange
        var ext = new ColorBlendExtension { Color = Colors.Red, Raito = 0.5d };

        // Act
        var converter = ext.ProvideValue(NullServiceProvider);

        // Assert
        Assert.Equal(Colors.Red, converter.Color);
        Assert.Equal(0.5d, converter.Raito);
    }
}
