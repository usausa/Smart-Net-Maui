namespace Smart.Maui.Markup;

public sealed class PrimitiveExtensionTest
{
    // [AcceptEmptyServiceProvider] means ProvideValue can be called with null
    private static readonly IServiceProvider NullServiceProvider = null!;

    [Fact]
    public void BoolExtensionTrue()
    {
        // Arrange
        var ext = new BoolExtension { Value = true };

        // Act & Assert
        Assert.Equal(true, ext.ProvideValue(NullServiceProvider));
    }

    [Fact]
    public void BoolExtensionFalse()
    {
        // Arrange
        var ext = new BoolExtension { Value = false };

        // Act & Assert
        Assert.Equal(false, ext.ProvideValue(NullServiceProvider));
    }

    [Fact]
    public void Int16ExtensionReturnsValue()
    {
        // Arrange
        var ext = new Int16Extension { Value = 42 };

        // Act & Assert
        Assert.Equal((short)42, ext.ProvideValue(NullServiceProvider));
    }

    [Fact]
    public void Int32ExtensionReturnsValue()
    {
        // Arrange
        var ext = new Int32Extension { Value = 100 };

        // Act & Assert
        Assert.Equal(100, ext.ProvideValue(NullServiceProvider));
    }

    [Fact]
    public void Int64ExtensionReturnsValue()
    {
        // Arrange
        var ext = new Int64Extension { Value = 9999L };

        // Act & Assert
        Assert.Equal(9999L, ext.ProvideValue(NullServiceProvider));
    }

    [Fact]
    public void FloatExtensionReturnsValue()
    {
        // Arrange
        var ext = new FloatExtension { Value = 1.5f };

        // Act & Assert
        Assert.Equal(1.5f, ext.ProvideValue(NullServiceProvider));
    }

    [Fact]
    public void DoubleExtensionReturnsValue()
    {
        // Arrange
        var ext = new DoubleExtension { Value = 3.14d };

        // Act & Assert
        Assert.Equal(3.14d, ext.ProvideValue(NullServiceProvider));
    }

    [Fact]
    public void EnumValuesExtensionReturnsArray()
    {
        // Arrange
        var ext = new EnumValuesExtension(typeof(DayOfWeek));

        // Act
        var result = ext.ProvideValue(NullServiceProvider);

        // Assert
        Assert.IsAssignableFrom<Array>(result);
    }
}
