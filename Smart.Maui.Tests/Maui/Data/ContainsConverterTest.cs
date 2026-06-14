namespace Smart.Maui.Data;

using System.Collections.Generic;
using System.Globalization;

public sealed class ContainsConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void ContainedValueReturnsTrue()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();
        var list = new List<string> { "a", "b", "c" };

        // Act
        var result = converter.Convert("b", typeof(bool), list, Culture);

        // Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void NotContainedValueReturnsFalse()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();
        var list = new List<string> { "a", "b" };

        // Act
        var result = converter.Convert("z", typeof(bool), list, Culture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void NullParameterReturnsFalse()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();

        // Act
        var result = converter.Convert("a", typeof(bool), null, Culture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new ContainsToBoolConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack(true, typeof(string), null, Culture));
    }
}
