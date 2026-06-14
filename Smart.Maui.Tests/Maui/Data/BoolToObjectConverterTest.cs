namespace Smart.Maui.Data;

using System.Globalization;

public sealed class BoolToObjectConverterTest
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void TrueValueReturnsTrue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act
        var result = converter.Convert(true, typeof(string), null, Culture);

        // Assert
        Assert.Equal("yes", result);
    }

    [Fact]
    public void FalseValueReturnsFalse()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act
        var result = converter.Convert(false, typeof(string), null, Culture);

        // Assert
        Assert.Equal("no", result);
    }

    [Fact]
    public void NonBoolReturnsDefault()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act
        var result = converter.Convert(null, typeof(string), null, Culture);

        // Assert
        Assert.Equal("no", result);
    }

    [Fact]
    public void ConvertBackTrueValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act
        var result = converter.ConvertBack("yes", typeof(bool), null, Culture);

        // Assert
        Assert.Equal(true, result);
    }

    [Fact]
    public void ConvertBackFalseValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act
        var result = converter.ConvertBack("no", typeof(bool), null, Culture);

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void ConvertBackUnknownReturnsDoNothing()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act
        var result = converter.ConvertBack("maybe", typeof(bool), null, Culture);

        // Assert
        Assert.Equal(Binding.DoNothing, result);
    }
}
