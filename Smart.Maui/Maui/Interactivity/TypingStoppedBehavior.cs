namespace Smart.Maui.Interactivity;

using System.Windows.Input;

using Microsoft.Maui.Dispatching;

public sealed class TypingStoppedBehavior : BehaviorBase<InputView>
{
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(TypingStoppedBehavior));

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(TypingStoppedBehavior));

    public static readonly BindableProperty DelayProperty = BindableProperty.Create(
        nameof(Delay),
        typeof(TimeSpan),
        typeof(TypingStoppedBehavior),
        TimeSpan.FromSeconds(1));

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

    public TimeSpan Delay
    {
        get => (TimeSpan)GetValue(DelayProperty);
        set => SetValue(DelayProperty, value);
    }

    private IDispatcherTimer? timer;

    protected override void OnAttachedTo(InputView bindable)
    {
        base.OnAttachedTo(bindable);

        bindable.TextChanged += OnTextChanged;
    }

    protected override void OnDetachingFrom(InputView bindable)
    {
        bindable.TextChanged -= OnTextChanged;
        if (timer is not null)
        {
            timer.Stop();
            timer.Tick -= OnTick;
            timer = null;
        }

        base.OnDetachingFrom(bindable);
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is InputView view)
        {
            if (timer is null)
            {
                timer = view.Dispatcher.CreateTimer();
                timer.IsRepeating = false;
                timer.Tick += OnTick;
            }

            timer.Stop();
            timer.Interval = Delay;
            timer.Start();
        }
    }

    private void OnTick(object? sender, EventArgs e)
    {
        var command = Command;
        if (command is null)
        {
            return;
        }

        var commandParameter = CommandParameter;
        var parameter = (commandParameter is not null) || IsSet(CommandParameterProperty) ? commandParameter : AssociatedObject?.Text;
        if (command.CanExecute(parameter))
        {
            command.Execute(parameter);
        }
    }
}
