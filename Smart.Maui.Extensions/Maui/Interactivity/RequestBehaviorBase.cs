namespace Smart.Maui.Interactivity;

using Smart.Mvvm.Messaging;

public abstract class RequestBehaviorBase<TEventArgs> : ActionBehaviorBase<BindableObject>
    where TEventArgs : EventArgs
{
    public static readonly BindableProperty RequestProperty = BindableProperty.Create(
        nameof(Request),
        typeof(IEventRequest<TEventArgs>),
        typeof(RequestBehaviorBase<TEventArgs>),
        propertyChanged: HandleRequestPropertyChanged);

    public IEventRequest<TEventArgs>? Request
    {
        get => (IEventRequest<TEventArgs>)GetValue(RequestProperty);
        set => SetValue(RequestProperty, value);
    }

    protected override void OnDetachingFrom(BindableObject bindable)
    {
        if (Request is not null)
        {
            Request.Requested -= EventRequestOnRequested;
        }

        base.OnDetachingFrom(bindable);
    }

    private static void HandleRequestPropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        ((RequestBehaviorBase<TEventArgs>)bindable).OnMessengerPropertyChanged(oldValue as IEventRequest<TEventArgs>, newValue as IEventRequest<TEventArgs>);
    }

    private void OnMessengerPropertyChanged(IEventRequest<TEventArgs>? oldValue, IEventRequest<TEventArgs>? newValue)
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

    private void EventRequestOnRequested(object? sender, TEventArgs e)
    {
        OnEventRequest(sender, e);
    }

    protected abstract void OnEventRequest(object? sender, TEventArgs e);
}
