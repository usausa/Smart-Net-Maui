namespace Smart.Maui.Interactivity;

using Microsoft.Maui.Controls;

public sealed class CallMethodBehaviorTests
{
    private sealed class MethodTarget
    {
        public int Count { get; private set; }

        public void Invoke() => Count++;
    }

    [Fact]
    public void CallsOwnTargetForEachAttachedObject()
    {
        // Arrange
        var first = new Button();
        var second = new Button();
        var firstTarget = new MethodTarget();
        var secondTarget = new MethodTarget();
        first.Behaviors.Add(new CallMethodBehavior { EventName = nameof(Button.Clicked), TargetObject = firstTarget, MethodName = nameof(MethodTarget.Invoke) });
        second.Behaviors.Add(new CallMethodBehavior { EventName = nameof(Button.Clicked), TargetObject = secondTarget, MethodName = nameof(MethodTarget.Invoke) });

        // Act
        ((IButtonController)second).SendClicked();

        // Assert
        Assert.Equal(0, firstTarget.Count);
        Assert.Equal(1, secondTarget.Count);
    }

    [Fact]
    public void ResubscribesWhenEventNameChangedToOtherHandlerType()
    {
        // Arrange
        var entry = new Entry();
        var target = new MethodTarget();
        var behavior = new CallMethodBehavior { EventName = nameof(Entry.Completed), TargetObject = target, MethodName = nameof(MethodTarget.Invoke) };
        entry.Behaviors.Add(behavior);

        // Act
        behavior.EventName = nameof(Entry.TextChanged);
        ((IEntryController)entry).SendCompleted();
        entry.Text = "a";

        // Assert
        Assert.Equal(1, target.Count);
    }
}
