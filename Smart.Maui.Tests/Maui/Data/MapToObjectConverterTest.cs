namespace Smart.Maui.Data;

using System.Globalization;

public sealed class MapToObjectConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ConvertMatchedEntryReturnsMappedValue()
    {
        // Arrange
        var converter = new MapToTextConverter { DefaultValue = "default" };
        converter.Entries.Add(new MapToTextEntry { Key = 1, Value = "one" });
        converter.Entries.Add(new MapToTextEntry { Key = 2, Value = "two" });

        // Act
        var result = converter.Convert(2, typeof(string), null, Culture);

        // Assert
        Assert.Equal("two", result);
    }

    [Fact]
    public void ConvertUnmatchedEntryReturnsDefaultValue()
    {
        // Arrange
        var converter = new MapToTextConverter { DefaultValue = "default" };
        converter.Entries.Add(new MapToTextEntry { Key = 1, Value = "one" });

        // Act
        var result = converter.Convert(9, typeof(string), null, Culture);

        // Assert
        Assert.Equal("default", result);
    }

    [Fact]
    public void NullInputReturnsDefault()
    {
        // Arrange
        var converter = new MapToTextConverter { DefaultValue = "default" };

        // Act
        var result = converter.Convert(null, typeof(string), null, Culture);

        // Assert
        Assert.Equal("default", result);
    }

    [Fact]
    public void NoMatchingEntryReturnsDefault()
    {
        // Arrange
        var converter = new MapToTextConverter { DefaultValue = "fallback" };

        // Act
        var result = converter.Convert(99, typeof(string), null, Culture);

        // Assert
        Assert.Equal("fallback", result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new MapToTextConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("one", typeof(int), null, Culture));
    }
}
