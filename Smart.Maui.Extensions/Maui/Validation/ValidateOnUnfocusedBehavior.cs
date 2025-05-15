namespace Smart.Maui.Validation;

using Smart.Maui.Interactivity;
using Smart.Maui.ViewModels;

public sealed class ValidateOnUnfocusedBehavior : BehaviorBase<VisualElement>
{
    public static readonly BindableProperty TargetProperty = BindableProperty.Create(
        nameof(Target),
        typeof(IValidatable),
        typeof(ValidateOnUnfocusedBehavior));

    public static readonly BindableProperty KeyProperty = BindableProperty.Create(
        nameof(Key),
        typeof(string),
        typeof(ValidateOnUnfocusedBehavior));

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

    protected override void OnAttachedTo(VisualElement bindable)
    {
        base.OnAttachedTo(bindable);

        bindable.Unfocused += OnUnfocused;
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        bindable.Unfocused -= OnUnfocused;

        base.OnDetachingFrom(bindable);
    }

    private void OnUnfocused(object? sender, FocusEventArgs e)
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
