namespace Smart.Maui.Interactivity;

public sealed class TimerTrigger : TriggerBase<BindableObject>
{
    public static readonly BindableProperty IntervalProperty = BindableProperty.Create(
        nameof(Interval),
        typeof(TimeSpan),
        typeof(TimerTrigger),
        TimeSpan.FromSeconds(1));

    public static readonly BindableProperty ParameterProperty = BindableProperty.Create(
        nameof(Parameter),
        typeof(object),
        typeof(TimerTrigger));

    public TimeSpan Interval
    {
        get => (TimeSpan)GetValue(IntervalProperty);
        set => SetValue(IntervalProperty, value);
    }

    public object Parameter
    {
        get => GetValue(ParameterProperty);
        set => SetValue(ParameterProperty, value);
    }

    private int running;

    protected override void OnAttachedTo(BindableObject bindable)
    {
        base.OnAttachedTo(bindable);

        bindable.Dispatcher.DispatchDelayed(Interval, OnTick);
    }

    protected override void OnDetachingFrom(BindableObject bindable)
    {
        Interlocked.Exchange(ref running, 0);

        base.OnDetachingFrom(bindable);
    }

    private void OnTick()
    {
        var state = Interlocked.Exchange(ref running, running) == 1;
        if (state)
        {
            InvokeActions(Parameter);

            AssociatedObject?.Dispatcher.DispatchDelayed(Interval, OnTick);
        }
    }
}
