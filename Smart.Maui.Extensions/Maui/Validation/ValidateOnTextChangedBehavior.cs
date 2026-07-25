namespace Smart.Maui.Validation;

using Microsoft.Maui.Dispatching;

using Smart.Maui.Interactivity;
using Smart.Maui.ViewModels;

public sealed class ValidateOnTextChangedBehavior : BehaviorBase<InputView>
{
    public static readonly BindableProperty TargetProperty = BindableProperty.Create(
        nameof(Target),
        typeof(IValidatable),
        typeof(ValidateOnTextChangedBehavior));

    public static readonly BindableProperty KeyProperty = BindableProperty.Create(
        nameof(Key),
        typeof(string),
        typeof(ValidateOnTextChangedBehavior));

    public static readonly BindableProperty DelayProperty = BindableProperty.Create(
        nameof(Delay),
        typeof(TimeSpan),
        typeof(ValidateOnTextChangedBehavior),
        TimeSpan.Zero);

    public IValidatable? Target
    {
        get => (IValidatable)GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    public string? Key
    {
        get => (string)GetValue(KeyProperty);
        set => SetValue(KeyProperty, value);
    }

    public TimeSpan Delay
    {
        get => (TimeSpan)GetValue(DelayProperty);
        set => SetValue(DelayProperty, value);
    }

    private IDispatcherTimer? timer;

    private string? pendingKey;

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

        pendingKey = null;

        base.OnDetachingFrom(bindable);
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (AssociatedObject is null)
        {
            return;
        }

        var key = Key ?? Behavior.GetKey(AssociatedObject);
        if (String.IsNullOrEmpty(key))
        {
            return;
        }

        var delay = Delay;
        if (delay <= TimeSpan.Zero)
        {
            Validate(key);
            return;
        }

        pendingKey = key;

        timer ??= CreateTimer();
        timer.Stop();
        timer.Interval = delay;
        timer.Start();
    }

    private IDispatcherTimer CreateTimer()
    {
        var created = AssociatedObject!.Dispatcher.CreateTimer();
        created.Tick += OnTick;
        return created;
    }

    private void OnTick(object? sender, EventArgs e)
    {
        timer?.Stop();

        var key = pendingKey;
        pendingKey = null;

        if (!String.IsNullOrEmpty(key))
        {
            Validate(key);
        }
    }

    private void Validate(string key)
    {
        var target = Target ?? (BindingContext as IValidatable);
        target?.Validate(key);
    }
}
