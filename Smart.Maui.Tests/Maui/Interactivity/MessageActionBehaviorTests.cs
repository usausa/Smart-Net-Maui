namespace Smart.Maui.Interactivity;

using System.Runtime.CompilerServices;

using Microsoft.Maui.Controls;

using Smart.Mvvm.Messaging;

public sealed class MessageActionBehaviorTests
{
    [Fact]
    public void ExecutesActionWhileLoaded()
    {
        // Arrange
        _ = TestDispatcher.Install();
        var messenger = new Messenger();
        var action = new RecordAction();
        var page = new ContentPage();
        page.Behaviors.Add(new MessageActionBehavior { Messenger = messenger, Actions = { action } });
        _ = new Window(page);

        // Act
        messenger.Send("message", 1);

        // Assert
        Assert.Equal([1], action.Parameters);
    }

    [Fact]
    public void IgnoresMessageBeforeLoaded()
    {
        // Arrange
        _ = TestDispatcher.Install();
        var messenger = new Messenger();
        var action = new RecordAction();
        var page = new ContentPage();
        page.Behaviors.Add(new MessageActionBehavior { Messenger = messenger, Actions = { action } });

        // Act
        messenger.Send("message", 1);

        // Assert
        Assert.Empty(action.Parameters);
    }

    [Fact]
    public void ExecutesActionWhenAttachedAfterLoaded()
    {
        // Arrange
        _ = TestDispatcher.Install();
        var messenger = new Messenger();
        var action = new RecordAction();
        var page = new ContentPage();
        _ = new Window(page);
        page.Behaviors.Add(new MessageActionBehavior { Messenger = messenger, Actions = { action } });

        // Act
        messenger.Send("message", 1);

        // Assert
        Assert.Equal([1], action.Parameters);
    }

    [Fact]
    public void IgnoresMessageAfterUnloaded()
    {
        // Arrange
        _ = TestDispatcher.Install();
        var messenger = new Messenger();
        var action = new RecordAction();
        var page = new ContentPage();
        page.Behaviors.Add(new MessageActionBehavior { Messenger = messenger, Actions = { action } });
        var window = new Window(page);
        messenger.Send("message", 1);

        // Act
        window.Page = new ContentPage();
        messenger.Send("message", 2);

        // Assert
        Assert.Equal([1], action.Parameters);
    }

    [Fact]
    public void ExecutesActionAgainWhenReloaded()
    {
        // Arrange
        _ = TestDispatcher.Install();
        var messenger = new Messenger();
        var action = new RecordAction();
        var page = new ContentPage();
        page.Behaviors.Add(new MessageActionBehavior { Messenger = messenger, Actions = { action } });
        var window = new Window(page);
        messenger.Send("message", 1);
        window.Page = new ContentPage();
        messenger.Send("message", 2);

        // Act
        window.Page = page;
        messenger.Send("message", 3);

        // Assert
        Assert.Equal([1, 3], action.Parameters);
    }

    [Fact]
    public void FollowsMessengerChangedWhileLoaded()
    {
        // Arrange
        _ = TestDispatcher.Install();
        var first = new Messenger();
        var second = new Messenger();
        var action = new RecordAction();
        var page = new ContentPage();
        var behavior = new MessageActionBehavior { Messenger = first, Actions = { action } };
        page.Behaviors.Add(behavior);
        _ = new Window(page);

        // Act
        behavior.Messenger = second;
        first.Send("message", 1);
        second.Send("message", 2);

        // Assert
        Assert.Equal([2], action.Parameters);
    }

    [Fact]
    public void IgnoresMessageAfterDetached()
    {
        // Arrange
        _ = TestDispatcher.Install();
        var messenger = new Messenger();
        var action = new RecordAction();
        var page = new ContentPage();
        var behavior = new MessageActionBehavior { Messenger = messenger, Actions = { action } };
        page.Behaviors.Add(behavior);
        _ = new Window(page);

        // Act
        page.Behaviors.Remove(behavior);
        messenger.Send("message", 1);

        // Assert
        Assert.Empty(action.Parameters);
    }

    [Fact]
    public void ReleasesPageAfterUnloaded()
    {
        // Arrange
        _ = TestDispatcher.Install();
        var messenger = new Messenger();
        var action = new RecordAction();
        var reference = OpenAndClose(messenger, action);

        // Act
        for (var i = 0; i < 3; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        messenger.Send("message", 2);

        // Assert
        Assert.False(reference.IsAlive);
        Assert.Equal([1], action.Parameters);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference OpenAndClose(Messenger messenger, RecordAction action)
    {
        var page = new ContentPage();
        page.Behaviors.Add(new MessageActionBehavior { Messenger = messenger, Actions = { action } });
        var window = new Window(page);
        messenger.Send("message", 1);
        window.Page = new ContentPage();
        return new WeakReference(page);
    }

    private sealed class RecordAction : IAction
    {
        public List<object?> Parameters { get; } = [];

        public void Execute(BindableObject associatedObject, object? parameter) => Parameters.Add(parameter);
    }
}
