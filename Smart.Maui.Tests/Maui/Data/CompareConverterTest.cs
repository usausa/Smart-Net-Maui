namespace Smart.Maui.Data;

using System.Globalization;

using Smart.Maui.Expressions;

public sealed class CompareConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void EqualReturnsTrueValue()
    {
        // Arrange
        var converter = new CompareToBoolConverter();

        // Act
        var result = converter.Convert(1, typeof(bool), 1, Culture);

        // Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void NotEqualReturnsFalseValue()
    {
        // Arrange
        var converter = new CompareToBoolConverter();

        // Act
        var result = converter.Convert(1, typeof(bool), 2, Culture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void LessThanExpressionApplied()
    {
        // Arrange
        var converter = new CompareToBoolConverter { Expression = CompareExpressions.LessThan };

        // Act & Assert
        Assert.Equal(true, converter.Convert(1, typeof(bool), 2, Culture));
        Assert.Equal(false, converter.Convert(3, typeof(bool), 2, Culture));
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new CompareToBoolConverter();

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack(true, typeof(int), null, Culture));
    }
}
