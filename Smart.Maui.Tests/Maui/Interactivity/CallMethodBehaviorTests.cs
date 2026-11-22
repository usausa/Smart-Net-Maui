namespace Smart.Maui.Interactivity;

using Microsoft.Maui.Controls;

public sealed class CallMethodBehaviorTests
{
    private interface IArgument;

    private sealed class Argument : IArgument;

    private sealed class MethodTarget
    {
        public int Count { get; private set; }

        public int OtherCount { get; private set; }

        public object? Received { get; private set; }

        public string Failure { get; } = "failed";

        public void Invoke() => Count++;

        public void InvokeOther() => OtherCount++;

        public void InvokeWith(object argument) => Received = argument;

        public void Fail() => throw new InvalidOperationException(Failure);

        public void FailWith(object argument) => throw new InvalidOperationException($"{Failure} {argument}");
    }

    private sealed class OtherTarget
    {
        public int Count { get; private set; }

        public void Invoke() => Count++;
    }

    private sealed class OverloadTarget
    {
        public object? Received { get; private set; }

        public void Select(IArgument argument) => Received = argument;

        public void Select(string text) => Received = text;
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

    [Fact]
    public void IgnoresEmptyEventName()
    {
        // Arrange
        var button = new Button();
        var behavior = new CallMethodBehavior { TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Invoke) };

        // Act
        var exception = Record.Exception(() => button.Behaviors.Add(behavior));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void PassesMethodParameterWhenSet()
    {
        // Arrange
        var button = new Button();
        var target = new MethodTarget();
        button.Behaviors.Add(new CallMethodBehavior { EventName = nameof(Button.Clicked), TargetObject = target, MethodName = nameof(MethodTarget.InvokeWith), MethodParameter = "parameter" });

        // Act
        ((IButtonController)button).SendClicked();

        // Assert
        Assert.Equal("parameter", target.Received);
    }

    [Fact]
    public void ResolvesAgainWhenMethodNameChanged()
    {
        // Arrange
        var button = new Button();
        var target = new MethodTarget();
        var behavior = new CallMethodBehavior { EventName = nameof(Button.Clicked), TargetObject = target, MethodName = nameof(MethodTarget.Invoke) };
        button.Behaviors.Add(behavior);
        ((IButtonController)button).SendClicked();

        // Act
        behavior.MethodName = nameof(MethodTarget.InvokeOther);
        ((IButtonController)button).SendClicked();

        // Assert
        Assert.Equal(1, target.Count);
        Assert.Equal(1, target.OtherCount);
    }

    [Fact]
    public void ResolvesAgainWhenTargetTypeChanged()
    {
        // Arrange
        var button = new Button();
        var first = new MethodTarget();
        var second = new OtherTarget();
        var behavior = new CallMethodBehavior { EventName = nameof(Button.Clicked), TargetObject = first, MethodName = nameof(MethodTarget.Invoke) };
        button.Behaviors.Add(behavior);
        ((IButtonController)button).SendClicked();

        // Act
        behavior.TargetObject = second;
        ((IButtonController)button).SendClicked();

        // Assert
        Assert.Equal(1, first.Count);
        Assert.Equal(1, second.Count);
    }

    [Fact]
    public void ResolvesAgainWhenMethodParameterTypeChanged()
    {
        // Arrange
        var button = new Button();
        var target = new OverloadTarget();
        var argument = new Argument();
        var behavior = new CallMethodBehavior { EventName = nameof(Button.Clicked), TargetObject = target, MethodName = nameof(OverloadTarget.Select), MethodParameter = argument };
        button.Behaviors.Add(behavior);
        ((IButtonController)button).SendClicked();
        var first = target.Received;

        // Act
        behavior.MethodParameter = "text";
        ((IButtonController)button).SendClicked();

        // Assert
        Assert.Same(argument, first);
        Assert.Equal("text", target.Received);
    }

    [Fact]
    public void ThrowsOriginalExceptionWhenMethodThrows()
    {
        // Arrange
        var button = new Button();
        button.Behaviors.Add(new CallMethodBehavior { EventName = nameof(Button.Clicked), TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Fail) });

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => ((IButtonController)button).SendClicked());

        // Assert
        Assert.Equal("failed", exception.Message);
    }

    [Fact]
    public void ThrowsOriginalExceptionWhenMethodWithArgumentThrows()
    {
        // Arrange
        var button = new Button();
        button.Behaviors.Add(new CallMethodBehavior { EventName = nameof(Button.Clicked), TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.FailWith), MethodParameter = "parameter" });

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => ((IButtonController)button).SendClicked());

        // Assert
        Assert.Equal("failed parameter", exception.Message);
    }
}
