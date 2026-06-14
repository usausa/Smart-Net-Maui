namespace Smart.Maui.Data;

using System.Globalization;

using Smart.Maui.Expressions;

public sealed class BinaryConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void AddExpressionApplied()
    {
        // Arrange
        var converter = new BinaryConverter { Expression = BinaryExpressions.Add };

        // Act
        var result = converter.Convert(3, typeof(int), 4, Culture);

        // Assert
        Assert.Equal(7, result);
    }

    [Fact]
    public void MaxExpressionApplied()
    {
        // Arrange
        var converter = new BinaryConverter { Expression = BinaryExpressions.Max };

        // Act
        var result = converter.Convert(10, typeof(int), 5, Culture);

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new BinaryConverter { Expression = BinaryExpressions.Add };

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack(7, typeof(int), 4, Culture));
    }
}
