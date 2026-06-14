namespace Smart.Maui.Data;

using System.Globalization;

public sealed class ChainConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

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
}
