namespace Smart.Maui.Interactivity;

public sealed class DefaultFocusBehavior : BehaviorBase<VisualElement>
{
    protected override void OnAttachedTo(VisualElement bindable)
    {
        base.OnAttachedTo(bindable);

        bindable.Dispatcher.Dispatch(() => bindable.Focus());

        if (bindable.IsLoaded)
        {
            FocusTarget(bindable);
        }
        else
        {
            bindable.Loaded += OnLoaded;
        }
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        bindable.Loaded -= OnLoaded;

        base.OnDetachingFrom(bindable);
    }

    private static void OnLoaded(object? sender, EventArgs e)
    {
        if (sender is VisualElement element)
        {
            element.Loaded -= OnLoaded;
            FocusTarget(element);
        }
    }

    private static void FocusTarget(VisualElement root)
    {
        var target = FindTarget(root);
        target?.Dispatcher.Dispatch(() => target.Focus());
    }

    private static VisualElement? FindTarget(IVisualTreeElement parent)
    {
        foreach (var child in parent.GetVisualChildren())
        {
            if (child is not VisualElement { IsEnabled: true, IsVisible: true } visual)
            {
                continue;
            }

            if (DefaultFocus.GetIsTarget(visual))
            {
                return visual;
            }

            var target = FindTarget(visual);
            if (target is not null)
            {
                return target;
            }
        }

        return null;
    }
}
