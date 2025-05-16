namespace Smart.Maui.Interactivity;

using Smart.Maui.Messaging;

public sealed class FocusControlBehavior : BehaviorBase<VisualElement>
{
    public static readonly BindableProperty ControllerProperty = BindableProperty.Create(
        nameof(Controller),
        typeof(FocusController),
        typeof(FocusControlBehavior),
        propertyChanged: HandleControllerPropertyChanged);

    public FocusController? Controller
    {
        get => (FocusController)GetValue(ControllerProperty);
        set => SetValue(ControllerProperty, value);
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        var controller = Controller;
        if (controller is not null)
        {
            controller.FocusRequested -= OnFocusRequested;
            controller.FindRequested -= OnFindRequested;
        }

        base.OnDetachingFrom(bindable);
    }

    private static void HandleControllerPropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        ((FocusControlBehavior)bindable).OnMessengerPropertyChanged(oldValue as FocusController, newValue as FocusController);
    }

    private void OnMessengerPropertyChanged(FocusController? oldValue, FocusController? newValue)
    {
        if (oldValue == newValue)
        {
            return;
        }

        if (oldValue is not null)
        {
            oldValue.FocusRequested -= OnFocusRequested;
            oldValue.FindRequested -= OnFindRequested;
        }

        if (newValue is not null)
        {
            newValue.FocusRequested += OnFocusRequested;
            newValue.FindRequested += OnFindRequested;
        }
    }

    private void OnFocusRequested(object? sender, EventArgs<string> e)
    {
        Dispatcher.Dispatch(() =>
        {
            var root = AssociatedObject;
            if (root is null)
            {
                return;
            }

            var target = FindTargetByName(root, e.Data);
            target?.Focus();
        });
    }

    public static VisualElement? FindTargetByName(VisualElement parent, string name)
    {
        if (!parent.IsEnabled || !parent.IsVisible)
        {
            return null;
        }

        if (parent is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                if (!child.IsEnabled || (child is not VisualElement { IsVisible: true } visual))
                {
                    continue;
                }

                var key = Behavior.GetKey(visual);
                if (key == name)
                {
                    return visual;
                }

                var result = FindTargetByName(visual, name);
                if (result != null)
                {
                    return result;
                }
            }
        }

        if (parent is IContentView { Content: VisualElement visualContent })
        {
            var result = FindTargetByName(visualContent, name);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    private void OnFindRequested(object? sender, FocusFindEventArgs e)
    {
        var root = AssociatedObject;
        if (root is null)
        {
            return;
        }

        e.Name = FindFocused(root);
    }

    public static string? FindFocused(VisualElement parent)
    {
        if (!parent.IsEnabled || !parent.IsVisible)
        {
            return null;
        }

        if (parent is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                if (!child.IsEnabled || (child is not VisualElement { IsVisible: true } visual))
                {
                    continue;
                }

                if (visual.IsFocused)
                {
                    var key = Behavior.GetKey(visual);
                    return key;
                }

                var result = FindFocused(visual);
                if (result != null)
                {
                    return result;
                }
            }
        }

        if (parent is IContentView { Content: VisualElement visualContent })
        {
            var result = FindFocused(visualContent);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}
