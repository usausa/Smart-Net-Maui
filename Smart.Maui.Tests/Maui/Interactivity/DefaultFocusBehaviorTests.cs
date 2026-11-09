namespace Smart.Maui.Interactivity;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;

public sealed class DefaultFocusBehaviorTests
{
    private static Entry Track(List<IView> requested) => new() { Handler = new FocusRecorder(requested) };

    private static Entry Target(List<IView> requested, bool isTarget = true)
    {
        var entry = Track(requested);
        DefaultFocus.SetIsTarget(entry, isTarget);
        return entry;
    }

    [Fact]
    public void FocusesAssociatedElement()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var requested = new List<IView>();
        var entry = Track(requested);
        _ = new ContentPage { Content = entry };

        // Act
        entry.Behaviors.Add(new DefaultFocusBehavior());
        dispatcher.RunPending();

        // Assert
        Assert.Equal([entry], requested);
    }

    [Fact]
    public void FocusesFirstTargetWhenLoaded()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var requested = new List<IView>();
        var layout = new VerticalStackLayout();
        var page = new ContentPage { Content = layout };
        page.Behaviors.Add(new DefaultFocusBehavior());
        var first = Target(requested);
        layout.Children.Add(Track(requested));
        layout.Children.Add(Target(requested, false));
        layout.Children.Add(first);
        layout.Children.Add(Target(requested));

        // Act
        _ = new Window(page);
        dispatcher.RunPending();

        // Assert
        Assert.Equal([first], requested);
    }

    [Fact]
    public void SkipsDisabledAndHiddenTargets()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var requested = new List<IView>();
        var disabled = Target(requested);
        disabled.IsEnabled = false;
        var hidden = Target(requested);
        hidden.IsVisible = false;
        var expected = Target(requested);
        var page = new ContentPage { Content = new VerticalStackLayout { disabled, hidden, expected } };
        page.Behaviors.Add(new DefaultFocusBehavior());

        // Act
        _ = new Window(page);
        dispatcher.RunPending();

        // Assert
        Assert.Equal([expected], requested);
    }

    [Fact]
    public void SkipsTargetsInsideHiddenContainer()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var requested = new List<IView>();
        var hidden = new VerticalStackLayout { IsVisible = false };
        hidden.Children.Add(Target(requested));
        var disabled = new VerticalStackLayout { IsEnabled = false };
        disabled.Children.Add(Target(requested));
        var expected = Target(requested);
        var page = new ContentPage { Content = new VerticalStackLayout { hidden, disabled, new Border { Content = expected } } };
        page.Behaviors.Add(new DefaultFocusBehavior());

        // Act
        _ = new Window(page);
        dispatcher.RunPending();

        // Assert
        Assert.Equal([expected], requested);
    }

    [Fact]
    public void UsesBoundTarget()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var requested = new List<IView>();
        var bound = Track(requested);
        bound.SetBinding(DefaultFocus.IsTargetProperty, new Binding(nameof(Model.IsNew)) { Source = new Model(false) });
        var expected = Target(requested);
        var page = new ContentPage { Content = new VerticalStackLayout { bound, expected } };
        page.Behaviors.Add(new DefaultFocusBehavior());

        // Act
        _ = new Window(page);
        dispatcher.RunPending();

        // Assert
        Assert.Equal([expected], requested);
    }

    [Fact]
    public void FocusesTargetWhenAttachedAfterLoaded()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var requested = new List<IView>();
        var expected = Target(requested);
        var page = new ContentPage { Content = new VerticalStackLayout { expected } };
        _ = new Window(page);

        // Act
        page.Behaviors.Add(new DefaultFocusBehavior());
        dispatcher.RunPending();

        // Assert
        Assert.Equal([expected], requested);
    }

    [Fact]
    public void FocusesNothingInsideWithoutTarget()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var requested = new List<IView>();
        var page = new ContentPage { Content = new VerticalStackLayout { Track(requested), Target(requested, false) } };
        page.Behaviors.Add(new DefaultFocusBehavior());

        // Act
        _ = new Window(page);
        dispatcher.RunPending();

        // Assert
        Assert.Empty(requested);
    }

    [Fact]
    public void IgnoresLoadedAfterDetached()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var requested = new List<IView>();
        var page = new ContentPage { Content = new VerticalStackLayout { Target(requested) } };
        var behavior = new DefaultFocusBehavior();
        page.Behaviors.Add(behavior);
        page.Behaviors.Remove(behavior);

        // Act
        _ = new Window(page);
        dispatcher.RunPending();

        // Assert
        Assert.Empty(requested);
    }

    private sealed class Model(bool isNew)
    {
        public bool IsNew { get; } = isNew;
    }

    private sealed class FocusRecorder(List<IView> requested)
        : ViewHandler<IView, object>(ViewMapper, new CommandMapper<IView, FocusRecorder>(ViewCommandMapper)
        {
            [nameof(IView.Focus)] = (_, view, args) =>
            {
                requested.Add(view);
                if (args is FocusRequest request)
                {
                    request.TrySetResult(true);
                }
            }
        })
    {
        protected override object CreatePlatformView() => new();
    }
}
