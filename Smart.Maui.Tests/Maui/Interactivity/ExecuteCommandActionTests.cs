namespace Smart.Maui.Interactivity;

using System.Globalization;

using Microsoft.Maui.Controls;

using Smart.Maui.Input;

public sealed class ExecuteCommandActionTests
{
    private sealed class TestBindable : BindableObject;

    private sealed class NullConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => null;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    [Fact]
    public void PassesNullWhenConverterReturnsNull()
    {
        // Arrange
        var executed = false;
        object? received = "initial";
        var action = new ExecuteCommandAction
        {
            Command = new DelegateCommand<object?>(x =>
            {
                executed = true;
                received = x;
            }),
            Converter = new NullConverter()
        };

        // Act
        action.Execute(new TestBindable(), "eventArgument");

        // Assert
        Assert.True(executed);
        Assert.Null(received);
    }
}
