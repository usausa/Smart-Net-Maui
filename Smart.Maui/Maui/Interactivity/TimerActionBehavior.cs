namespace Smart.Maui.Interactivity;

using Microsoft.Maui.Dispatching;

public sealed class TimerActionBehavior : ActionBehaviorBase<VisualElement>
{
    public static readonly BindableProperty IntervalProperty = BindableProperty.Create(
        nameof(Interval),
        typeof(TimeSpan),
        typeof(TimerActionBehavior),
        TimeSpan.FromSeconds(1),
        propertyChanged: OnIntervalChanged);

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

    protected override void OnAttachedTo(VisualElement bindable)
    {
        base.OnAttachedTo(bindable);
        bindable.Loaded += OnLoaded;
        bindable.Unloaded += OnUnloaded;
        if (bindable.IsLoaded)
        {
            Start(bindable);
        }
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        if (timer is not null)
        {
            timer.Stop();
            timer.Tick -= OnTick;
            timer = null;
        }

        bindable.Loaded -= OnLoaded;
        bindable.Unloaded -= OnUnloaded;
        base.OnDetachingFrom(bindable);
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        if (sender is VisualElement element)
        {
            Start(element);
        }
    }

    private void OnUnloaded(object? sender, EventArgs e)
    {
        timer?.Stop();
    }

    private void Start(VisualElement element)
    {
        if (timer is null)
        {
            timer = element.Dispatcher.CreateTimer();
            timer.Tick += OnTick;
        }

        timer.Interval = Interval;
        timer.Start();
    }

    private void OnTick(object? sender, EventArgs e)
    {
        InvokeActions(Parameter);
    }

    private static void OnIntervalChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var behavior = (TimerActionBehavior)bindable;
        behavior.timer?.Interval = (TimeSpan)newValue;
    }
}
