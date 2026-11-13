namespace Smart.Maui.Interactivity;

using System.Globalization;

using Microsoft.Maui.Controls;

using Smart.Maui.Input;

public sealed class EventToCommandBehaviorTests
{
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
        var button = new Button();
        button.Behaviors.Add(new EventToCommandBehavior
        {
            EventName = nameof(Button.Clicked),
            Command = new DelegateCommand<object?>(x =>
            {
                executed = true;
                received = x;
            }),
            Converter = new NullConverter()
        });

        // Act
        ((IButtonController)button).SendClicked();

        // Assert
        Assert.True(executed);
        Assert.Null(received);
    }

    [Fact]
    public void ExecutesOwnCommandForEachAttachedObject()
    {
        // Arrange
        var first = new Button();
        var second = new Button();
        var firstCount = 0;
        var secondCount = 0;
        first.Behaviors.Add(new EventToCommandBehavior { EventName = nameof(Button.Clicked), Command = new DelegateCommand(() => firstCount++) });
        second.Behaviors.Add(new EventToCommandBehavior { EventName = nameof(Button.Clicked), Command = new DelegateCommand(() => secondCount++) });

        // Act
        ((IButtonController)second).SendClicked();

        // Assert
        Assert.Equal(0, firstCount);
        Assert.Equal(1, secondCount);
    }

    [Fact]
    public void ResubscribesWhenEventNameChangedToOtherHandlerType()
    {
        // Arrange
        var entry = new Entry();
        var received = new List<object?>();
        var behavior = new EventToCommandBehavior { EventName = nameof(Entry.Completed), Command = new DelegateCommand<object?>(received.Add) };
        entry.Behaviors.Add(behavior);

        // Act
        behavior.EventName = nameof(Entry.TextChanged);
        ((IEntryController)entry).SendCompleted();
        entry.Text = "a";

        // Assert
        Assert.IsType<TextChangedEventArgs>(Assert.Single(received));
    }
}
