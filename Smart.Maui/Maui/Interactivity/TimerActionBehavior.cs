namespace Smart.Maui.Interactivity;

using Microsoft.Maui.Dispatching;

public sealed class TimerActionBehavior : ActionBehaviorBase<BindableObject>
{
    public static readonly BindableProperty IntervalProperty = BindableProperty.Create(
        nameof(Interval),
        typeof(TimeSpan),
        typeof(TimerActionBehavior),
        TimeSpan.FromSeconds(1));

    public static readonly BindableProperty ParameterProperty = BindableProperty.Create(
        nameof(Parameter),
        typeof(object),
        typeof(TimerActionBehavior));

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

    private IDispatcherTimer? timer;

    protected override void OnAttachedTo(BindableObject bindable)
    {
        base.OnAttachedTo(bindable);

        timer = bindable.Dispatcher.CreateTimer();
        timer.Interval = Interval;
        timer.Tick += OnTick;
        timer.Start();
    }

    protected override void OnDetachingFrom(BindableObject bindable)
    {
        timer?.Stop();
        timer = null;

        base.OnDetachingFrom(bindable);
    }

    private void OnTick(object? sender, EventArgs e)
    {
        InvokeActions(Parameter);
    }
}
