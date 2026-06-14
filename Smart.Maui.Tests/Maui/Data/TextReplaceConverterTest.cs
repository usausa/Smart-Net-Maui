namespace Smart.Maui.Data;

using System.Globalization;
using System.Text.RegularExpressions;

public sealed class TextReplaceConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ReplacesAllOccurrences()
    {
        // Arrange
        var converter = new TextReplaceConverter
        {
            Pattern = "o",
            Replacement = "0",
            ReplaceAll = true
        };

        // Act
        var result = converter.Convert("foo bar boo", typeof(string), null, Culture);

        // Assert
        Assert.Equal("f00 bar b00", result);
    }

    [Fact]
    public void ReplaceFirstOccurrenceOnly()
    {
        // Arrange
        var converter = new TextReplaceConverter
        {
            Pattern = "o",
            Replacement = "0",
            ReplaceAll = false
        };

        // Act
        var result = converter.Convert("foo", typeof(string), null, Culture);

        // Assert
        Assert.Equal("f0o", result);
    }

    [Fact]
    public void NullInputReturnsNull()
    {
        // Arrange
        var converter = new TextReplaceConverter { Pattern = "x", Replacement = "y" };

        // Act
        var result = converter.Convert(null, typeof(string), null, Culture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void EmptyStringReturnsEmptyString()
    {
        // Arrange
        var converter = new TextReplaceConverter { Pattern = "x", Replacement = "y" };

        // Act
        var result = converter.Convert(string.Empty, typeof(string), null, Culture);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void OptionsApplied()
    {
        // Arrange
        var converter = new TextReplaceConverter
        {
            Pattern = "HELLO",
            Replacement = "world",
            Options = RegexOptions.IgnoreCase,
            ReplaceAll = true
        };

        // Act
        var result = converter.Convert("hello Hello HELLO", typeof(string), null, Culture);

        // Assert
        Assert.Equal("world world world", result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new TextReplaceConverter { Pattern = "x", Replacement = "y" };

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("text", typeof(string), null, Culture));
    }
}
