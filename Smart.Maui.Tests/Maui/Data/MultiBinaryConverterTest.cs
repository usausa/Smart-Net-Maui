namespace Smart.Maui.Data;

using System.Globalization;

using Smart.Maui.Expressions;

public sealed class MultiBinaryConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void AddMultipleValues()
    {
        // Arrange
        var converter = new MultiBinaryConverter { Expression = BinaryExpressions.Add };

        // Act
        var result = converter.Convert([1, 2, 3], typeof(int), null, Culture);

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void MaxOfMultipleValues()
    {
        // Arrange
        var converter = new MultiBinaryConverter { Expression = BinaryExpressions.Max };

        // Act
        var result = converter.Convert([3, 9, 5], typeof(int), null, Culture);

        // Assert
        Assert.Equal(9, result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new MultiBinaryConverter { Expression = BinaryExpressions.Add };

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack(6, [typeof(int)], null!, Culture));
    }
}
