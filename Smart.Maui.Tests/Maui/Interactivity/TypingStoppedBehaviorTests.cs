namespace Smart.Maui.Interactivity;

using Microsoft.Maui.Controls;

using Smart.Maui.Input;

public sealed class TypingStoppedBehaviorTests
{
    [Fact]
    public void ExecutesCommandWithTextAfterDelay()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var entry = new Entry();
        object? received = null;
        entry.Behaviors.Add(new TypingStoppedBehavior
        {
            Command = new DelegateCommand<object?>(x => received = x),
            Delay = TimeSpan.FromMilliseconds(800)
        });

        // Act
        entry.Text = "abc";
        var timer = Assert.Single(dispatcher.Timers);
        timer.Fire();

        // Assert
        Assert.Equal(TimeSpan.FromMilliseconds(800), timer.Interval);
        Assert.Equal("abc", received);
    }

    [Fact]
    public void ExecutesCommandOnceWithLatestText()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var entry = new Entry();
        var received = new List<object?>();
        entry.Behaviors.Add(new TypingStoppedBehavior { Command = new DelegateCommand<object?>(received.Add) });

        // Act
        entry.Text = "a";
        entry.Text = "ab";
        var timer = Assert.Single(dispatcher.Timers);
        timer.Fire();
        timer.Fire();

        // Assert
        Assert.Equal(["ab"], received);
    }

    [Fact]
    public void UsesCommandParameterWhenSet()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var entry = new Entry();
        object? received = null;
        entry.Behaviors.Add(new TypingStoppedBehavior
        {
            Command = new DelegateCommand<object?>(x => received = x),
            CommandParameter = "parameter"
        });

        // Act
        entry.Text = "abc";
        Assert.Single(dispatcher.Timers).Fire();

        // Assert
        Assert.Equal("parameter", received);
    }
}
