namespace Smart.Maui.Data;

using System.ComponentModel;
using System.Globalization;

public sealed class EnumDescriptionConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    private enum SampleEnum
    {
        [Description("First Item")]
        First,
        Second
    }

    [Fact]
    public void EnumWithDescriptionReturnsDescription()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act
        var result = converter.Convert(SampleEnum.First, typeof(string), null, Culture);

        // Assert
        Assert.Equal("First Item", result);
    }

    [Fact]
    public void EnumWithoutDescriptionReturnsToString()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act
        var result = converter.Convert(SampleEnum.Second, typeof(string), null, Culture);

        // Assert
        Assert.Equal("Second", result);
    }

    [Fact]
    public void NullReturnsNull()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act
        var result = converter.Convert(null, typeof(string), null, Culture);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new EnumDescriptionConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("First Item", typeof(SampleEnum), null, Culture));
    }
}
