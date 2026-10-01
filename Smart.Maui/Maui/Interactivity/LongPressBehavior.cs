namespace Smart.Maui.Interactivity;

using System.Windows.Input;

using Microsoft.Maui.Dispatching;

using Smart.Maui.Internal;

public sealed class LongPressBehavior : BehaviorBase<View>
{
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(LongPressBehavior));

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(LongPressBehavior));

    public static readonly BindableProperty DurationProperty = BindableProperty.Create(
        nameof(Duration),
        typeof(TimeSpan),
        typeof(LongPressBehavior),
        TimeSpan.FromMilliseconds(500));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public TimeSpan Duration
    {
        get => (TimeSpan)GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    private IDispatcherTimer? timer;

    protected override void OnAttachedTo(View bindable)
    {
        base.OnAttachedTo(bindable);

        PressEvents.Add(bindable, OnPressed, OnReleased);
    }

    protected override void OnDetachingFrom(View bindable)
    {
        PressEvents.Remove(bindable, OnPressed, OnReleased);
        if (timer is not null)
        {
            timer.Stop();
            timer.Tick -= OnTick;
            timer = null;
        }

        base.OnDetachingFrom(bindable);
    }

    private void OnPressed(object? sender, EventArgs e)
    {
        if (sender is View view)
        {
            if (timer is null)
            {
                timer = view.Dispatcher.CreateTimer();
                timer.IsRepeating = false;
                timer.Tick += OnTick;
            }

            timer.Interval = Duration;
            timer.Start();
        }
    }

    private void OnReleased(object? sender, EventArgs e)
    {
        timer?.Stop();
    }

    private void OnTick(object? sender, EventArgs e)
    {
        var command = Command;
        if (command is null)
        {
            return;
        }

        var parameter = CommandParameter;
        if (command.CanExecute(parameter))
        {
            command.Execute(parameter);
        }
    }
}
