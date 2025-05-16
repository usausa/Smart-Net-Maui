namespace Smart.Maui.Interactivity;

using Smart.Mvvm.Messaging;

public sealed class FocusControlBehavior : BehaviorBase<VisualElement>
{
    public static readonly BindableProperty RequestProperty = BindableProperty.Create(
        nameof(Request),
        typeof(IEventRequest<ParameterEventArgs>),
        typeof(FocusControlBehavior),
        propertyChanged: HandleRequestPropertyChanged);

    public IEventRequest<ParameterEventArgs>? Request
    {
        get => (IEventRequest<ParameterEventArgs>)GetValue(RequestProperty);
        set => SetValue(RequestProperty, value);
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        var request = Request;
        if (request is not null)
        {
            request.Requested -= EventRequestOnRequested;
        }

        base.OnDetachingFrom(bindable);
    }

    private static void HandleRequestPropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        ((FocusControlBehavior)bindable).OnMessengerPropertyChanged(oldValue as IEventRequest<ParameterEventArgs>, newValue as IEventRequest<ParameterEventArgs>);
    }

    private void OnMessengerPropertyChanged(IEventRequest<ParameterEventArgs>? oldValue, IEventRequest<ParameterEventArgs>? newValue)
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

    private void EventRequestOnRequested(object? sender, ParameterEventArgs e)
    {
        Dispatcher.Dispatch(() =>
        {
            var root = AssociatedObject;
            if (root is null)
            {
                return;
            }

            var target = FindTarget(root, (e.Parameter as string)!);
            target?.Focus();
        });
    }

    public static VisualElement? FindTarget(VisualElement parent, string name)
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

                if (child is Layout childLayout)
                {
                    var result = FindTarget(childLayout, name);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
        }

        if ((parent is IContentView view) && (view.Content is VisualElement visualContent))
        {
            var result = FindTarget(visualContent, name);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}
