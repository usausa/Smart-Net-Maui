namespace Smart.Maui.Interactivity;

using Microsoft.Maui.Controls;

public sealed class TimerActionBehaviorTests
{
    [Fact]
    public void DoesNotStartBeforeLoaded()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var page = new ContentPage();

        // Act
        page.Behaviors.Add(new TimerActionBehavior());

        // Assert
        Assert.Empty(dispatcher.Timers);
    }

    [Fact]
    public void ExecutesActionsWhileLoaded()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var action = new RecordAction();
        var page = new ContentPage();
        page.Behaviors.Add(new TimerActionBehavior { Interval = TimeSpan.FromSeconds(2), Parameter = "parameter", Actions = { action } });
        _ = new Window(page);
        var timer = Assert.Single(dispatcher.Timers);

        // Act
        timer.Fire();

        // Assert
        Assert.Equal(TimeSpan.FromSeconds(2), timer.Interval);
        Assert.Equal(["parameter"], action.Parameters);
    }

    [Fact]
    public void StartsWhenAttachedAfterLoaded()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var page = new ContentPage();
        _ = new Window(page);

        // Act
        page.Behaviors.Add(new TimerActionBehavior());

        // Assert
        var timer = Assert.Single(dispatcher.Timers);
        Assert.True(timer.IsRunning);
    }

    [Fact]
    public void StopsAfterUnloaded()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var action = new RecordAction();
        var page = new ContentPage();
        page.Behaviors.Add(new TimerActionBehavior { Actions = { action } });
        var window = new Window(page);
        var timer = Assert.Single(dispatcher.Timers);

        // Act
        window.Page = new ContentPage();
        timer.Fire();

        // Assert
        Assert.False(timer.IsRunning);
        Assert.Empty(action.Parameters);
    }

    [Fact]
    public void RestartsWhenReloaded()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var action = new RecordAction();
        var page = new ContentPage();
        page.Behaviors.Add(new TimerActionBehavior { Actions = { action } });
        var window = new Window(page) { Page = new ContentPage() };

        // Act
        window.Page = page;
        var timer = Assert.Single(dispatcher.Timers);
        timer.Fire();

        // Assert
        Assert.True(timer.IsRunning);
        Assert.Single(action.Parameters);
    }

    [Fact]
    public void StopsAfterDetached()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var page = new ContentPage();
        var behavior = new TimerActionBehavior();
        page.Behaviors.Add(behavior);
        _ = new Window(page);
        var timer = Assert.Single(dispatcher.Timers);

        // Act
        page.Behaviors.Remove(behavior);

        // Assert
        Assert.False(timer.IsRunning);
    }

    [Fact]
    public void AppliesIntervalChanged()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var page = new ContentPage();
        var behavior = new TimerActionBehavior();
        page.Behaviors.Add(behavior);
        _ = new Window(page);
        var timer = Assert.Single(dispatcher.Timers);

        // Act
        behavior.Interval = TimeSpan.FromSeconds(3);

        // Assert
        Assert.Equal(TimeSpan.FromSeconds(3), timer.Interval);
    }

    private sealed class RecordAction : IAction
    {
        public List<object?> Parameters { get; } = [];

        public void Execute(BindableObject associatedObject, object? parameter) => Parameters.Add(parameter);
    }
}
