namespace Smart.Maui.Data;

using System.Globalization;

public sealed class MapToObjectConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    // NOTE: MapToObjectConverter<T>.Entries is an empty read-only array (no Add/setter exposed).
    // Only the DefaultValue path and ConvertBack are testable without reflection.

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
