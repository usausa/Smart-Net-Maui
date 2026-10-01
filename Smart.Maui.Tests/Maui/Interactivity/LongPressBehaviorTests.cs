namespace Smart.Maui.Interactivity;

using Microsoft.Maui.Controls;

using Smart.Maui.Input;

public sealed class LongPressBehaviorTests
{
    [Fact]
    public void ExecutesCommandWhenHeldForDuration()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var button = new Button();
        object? received = null;
        var behavior = new LongPressBehavior
        {
            Command = new DelegateCommand<object?>(x => received = x),
            CommandParameter = "parameter",
            Duration = TimeSpan.FromMilliseconds(300)
        };
        button.Behaviors.Add(behavior);

        // Act
        ((IButtonController)button).SendPressed();
        var timer = Assert.Single(dispatcher.Timers);
        timer.Fire();

        // Assert
        Assert.Equal(TimeSpan.FromMilliseconds(300), timer.Interval);
        Assert.Equal("parameter", received);
    }

    [Fact]
    public void DoesNotExecuteCommandWhenReleasedBeforeDuration()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var button = new Button();
        var count = 0;
        button.Behaviors.Add(new LongPressBehavior { Command = new DelegateCommand(() => count++) });

        // Act
        ((IButtonController)button).SendPressed();
        ((IButtonController)button).SendReleased();
        Assert.Single(dispatcher.Timers).Fire();

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public void ExecutesCommandForImageButton()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var button = new ImageButton();
        var count = 0;
        button.Behaviors.Add(new LongPressBehavior { Command = new DelegateCommand(() => count++) });

        // Act
        ((IButtonController)button).SendPressed();
        Assert.Single(dispatcher.Timers).Fire();

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void DoesNotExecuteCommandAfterDetached()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var button = new Button();
        var count = 0;
        var behavior = new LongPressBehavior { Command = new DelegateCommand(() => count++) };
        button.Behaviors.Add(behavior);
        ((IButtonController)button).SendPressed();

        // Act
        button.Behaviors.Remove(behavior);
        Assert.Single(dispatcher.Timers).Fire();

        // Assert
        Assert.Equal(0, count);
    }
}
