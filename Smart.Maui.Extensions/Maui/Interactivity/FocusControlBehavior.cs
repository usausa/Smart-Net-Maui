namespace Smart.Maui.Interactivity;

using Smart.Mvvm.Messaging;

public sealed class FocusControlBehavior : BehaviorBase<Layout>
{
    public static readonly BindableProperty RequestProperty = BindableProperty.Create(
        nameof(Request),
        typeof(IEventRequest<EventArgs<string>>),
        typeof(FocusControlBehavior),
        propertyChanged: HandleRequestPropertyChanged);

    public IEventRequest<EventArgs<string>>? Request
    {
        get => (IEventRequest<EventArgs<string>>)GetValue(RequestProperty);
        set => SetValue(RequestProperty, value);
    }

    protected override void OnDetachingFrom(Layout bindable)
    {
        if (Request is not null)
        {
            Request.Requested -= EventRequestOnRequested;
        }

        base.OnDetachingFrom(bindable);
    }

    private static void HandleRequestPropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        ((FocusControlBehavior)bindable).OnMessengerPropertyChanged(oldValue as IEventRequest<EventArgs<string>>, newValue as IEventRequest<EventArgs<string>>);
    }

    private void OnMessengerPropertyChanged(IEventRequest<EventArgs<string>>? oldValue, IEventRequest<EventArgs<string>>? newValue)
    {
        if (oldValue == newValue)
        {
            return;
        }

        if (oldValue is not null)
        {
            oldValue.Requested -= EventRequestOnRequested;
        }

        if (newValue is not null)
        {
            newValue.Requested += EventRequestOnRequested;
        }
    }

    private void EventRequestOnRequested(object? sender, EventArgs<string> e)
    {
        Dispatcher.Dispatch(() =>
        {
            if (AssociatedObject is null)
            {
                return;
            }

            var target = FindTarget(AssociatedObject, e.Data);
            target?.Focus();
        });
    }

    public static VisualElement? FindTarget(Layout layout, string name)
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

            if (child is Layout childLayout)
            {
                var result = FindTarget(childLayout, name);
                if (result != null)
                {
                    return result;
                }
            }
        }

        return null;
    }
}
