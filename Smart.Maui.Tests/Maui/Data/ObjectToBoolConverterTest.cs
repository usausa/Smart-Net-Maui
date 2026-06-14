namespace Smart.Maui.Data;

using System.Globalization;

public sealed class ObjectToBoolConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void MatchingValueReturnsTrue()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "active", FalseValue = "inactive" };

        // Act
        var result = converter.Convert("active", typeof(bool), null, Culture);

        // Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void NonMatchingValueReturnsFalse()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "active", FalseValue = "inactive" };

        // Act
        var result = converter.Convert("inactive", typeof(bool), null, Culture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void NullValueReturnsFalse()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "active", FalseValue = "inactive" };

        // Act
        var result = converter.Convert(null, typeof(bool), null, Culture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void ConvertBackTrueReturnsTrueValue()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "active", FalseValue = "inactive" };

        // Act
        var result = converter.ConvertBack(true, typeof(string), null, Culture);

        // Assert
        Assert.Equal("active", result);
    }

    [Fact]
    public void ConvertBackFalseReturnsFalseValue()
    {
        // Arrange
        var converter = new TextToBoolConverter { TrueValue = "active", FalseValue = "inactive" };

        // Act
        var result = converter.ConvertBack(false, typeof(string), null, Culture);

        // Assert
        Assert.Equal("inactive", result);
    }

    [Fact]
    public void IntToBoolMatchingReturnsTrue()
    {
        // Arrange
        var converter = new IntToBoolConverter { TrueValue = 1, FalseValue = 0 };

        // Act
        var result = converter.Convert(1, typeof(bool), null, Culture);

        // Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void IntToBoolNonMatchingReturnsFalse()
    {
        // Arrange
        var converter = new IntToBoolConverter { TrueValue = 1, FalseValue = 0 };

        // Act
        var result = converter.Convert(99, typeof(bool), null, Culture);

        // Assert
        Assert.Equal(false, result);
    }
}
