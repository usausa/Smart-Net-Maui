namespace Smart.Maui.Validation;

using Smart.Maui.Interactivity;
using Smart.Maui.ViewModels;
using Smart.Mvvm.ViewModels;

public sealed class ClearErrorOnFocusBehavior : BehaviorBase<VisualElement>
{
    public static readonly BindableProperty TargetProperty = BindableProperty.Create(
        nameof(Target),
        typeof(ErrorInfo),
        typeof(ClearErrorOnFocusBehavior));

    public static readonly BindableProperty KeyProperty = BindableProperty.Create(
        nameof(Key),
        typeof(string),
        typeof(ClearErrorOnFocusBehavior));

    public ErrorInfo? Target
    {
        get => (ErrorInfo)GetValue(TargetProperty);
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

        bindable.Focused += OnFocused;
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        bindable.Focused -= OnFocused;

        base.OnDetachingFrom(bindable);
    }

    private void OnFocused(object? sender, FocusEventArgs e)
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

        var errorInfo = Target ?? (BindingContext is ExtendViewModelBase vm ? vm.Errors : null);
        errorInfo?.ClearErrors(key);
    }
}
