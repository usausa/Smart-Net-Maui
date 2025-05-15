namespace Smart.Maui.Validation;

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

    protected override void OnAttachedTo(InputView bindable)
    {
        base.OnAttachedTo(bindable);

        bindable.TextChanged += OnTextChanged;
    }

    protected override void OnDetachingFrom(InputView bindable)
    {
        bindable.TextChanged -= OnTextChanged;

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

        Target?.Validate(key);
    }
}
