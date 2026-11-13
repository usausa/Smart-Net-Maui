namespace Smart.Maui.Interactivity;

using Microsoft.Maui.Controls;

public sealed class EventActionBehaviorTests
{
    private sealed class RecordAction : IAction
    {
        public List<(BindableObject AssociatedObject, object? Parameter)> Calls { get; } = [];

        public void Execute(BindableObject associatedObject, object? parameter) => Calls.Add((associatedObject, parameter));
    }

    [Fact]
    public void ExecutesOwnActionsForEachAttachedObject()
    {
        // Arrange
        var first = new Button();
        var second = new Button();
        var firstAction = new RecordAction();
        var secondAction = new RecordAction();
        var firstBehavior = new EventActionBehavior { EventName = nameof(Button.Clicked) };
        firstBehavior.Actions.Add(firstAction);
        first.Behaviors.Add(firstBehavior);
        var secondBehavior = new EventActionBehavior { EventName = nameof(Button.Clicked) };
        secondBehavior.Actions.Add(secondAction);
        second.Behaviors.Add(secondBehavior);

        // Act
        ((IButtonController)second).SendClicked();

        // Assert
        Assert.Empty(firstAction.Calls);
        var (associatedObject, parameter) = Assert.Single(secondAction.Calls);
        Assert.Same(second, associatedObject);
        Assert.Same(EventArgs.Empty, parameter);
    }

    [Fact]
    public void ResubscribesWhenEventNameChangedToOtherHandlerType()
    {
        // Arrange
        var entry = new Entry();
        var action = new RecordAction();
        var behavior = new EventActionBehavior { EventName = nameof(Entry.Completed) };
        behavior.Actions.Add(action);
        entry.Behaviors.Add(behavior);

        // Act
        behavior.EventName = nameof(Entry.TextChanged);
        ((IEntryController)entry).SendCompleted();
        entry.Text = "a";

        // Assert
        var (associatedObject, parameter) = Assert.Single(action.Calls);
        Assert.Same(entry, associatedObject);
        Assert.IsType<TextChangedEventArgs>(parameter);
    }
}
