namespace Smart.Maui.Data;

using System.Globalization;

public sealed class ChainConverterTests
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    private sealed class DoNothingConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => Binding.DoNothing;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => Binding.DoNothing;
    }

    [Fact]
    public void EmptyChainReturnsInput()
    {
        // Arrange
        var converter = new ChainConverter();

        // Act
        var result = converter.Convert("hello", typeof(string), null, Culture);

        // Assert
        Assert.Equal("hello", result);
    }

    [Fact]
    public void SingleConverterApplied()
    {
        // Arrange
        var converter = new ChainConverter();
        converter.Converters.Add(new ToUpperConverter());

        // Act
        var result = converter.Convert("hello", typeof(string), null, Culture);

        // Assert
        Assert.Equal("HELLO", result);
    }

    [Fact]
    public void MultipleConvertersAppliedInOrder()
    {
        // Arrange
        var converter = new ChainConverter();
        converter.Converters.Add(new ToUpperConverter());
        converter.Converters.Add(new ToLowerConverter());

        // Act
        var result = converter.Convert("Hello", typeof(string), null, Culture);

        // Assert
        Assert.Equal("hello", result);
    }

    [Fact]
    public void ConvertBackAppliedInReverseOrder()
    {
        // Arrange
        var converter = new ChainConverter();
        converter.Converters.Add(new ToUpperConverter());

        // Act & Assert
        // ToUpperConverter.ConvertBack throws NotSupportedException
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("HELLO", typeof(string), null, Culture));
    }

    [Fact]
    public void ConvertStopsAtDoNothing()
    {
        // Arrange
        var converter = new ChainConverter();
        converter.Converters.Add(new DoNothingConverter());
        converter.Converters.Add(new NullToTextConverter { NullValue = "null", NonNullValue = "set" });

        // Act
        var result = converter.Convert("hello", typeof(string), null, Culture);

        // Assert
        Assert.Equal(Binding.DoNothing, result);
    }

    [Fact]
    public void ConvertBackStopsAtDoNothing()
    {
        // Arrange
        var converter = new ChainConverter();
        converter.Converters.Add(new ObjectConvertConverter());
        converter.Converters.Add(new BoolToTextConverter { TrueValue = "ON", FalseValue = "OFF" });

        // Act
        // BoolToTextConverter.ConvertBack returns DoNothing, which must not reach ObjectConvertConverter
        var result = converter.ConvertBack("unknown", typeof(int), null, Culture);

        // Assert
        Assert.Equal(Binding.DoNothing, result);
    }
}
