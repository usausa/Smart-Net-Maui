namespace Smart.Maui.Interactivity;

using System.Globalization;

using Microsoft.Maui.Controls;

public sealed class ChangePropertyActionTests
{
    private sealed class TestBindable : BindableObject;

    private sealed class NullConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => null;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    private sealed class PropertyTarget
    {
        public object? Value { get; set; } = "initial";
    }

    [Fact]
    public void SetsNullWhenConverterReturnsNull()
    {
        // Arrange
        var target = new PropertyTarget();
        var action = new ChangePropertyAction
        {
            TargetObject = target,
            PropertyName = nameof(PropertyTarget.Value),
            Converter = new NullConverter()
        };

        // Act
        action.Execute(new TestBindable(), "eventArgument");

        // Assert
        Assert.Null(target.Value);
    }
}
