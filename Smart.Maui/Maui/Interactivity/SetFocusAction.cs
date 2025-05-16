namespace Smart.Maui.Interactivity;

public sealed class SetFocusAction : BindableObject, IAction
{
    public static readonly BindableProperty TargetObjectProperty = BindableProperty.Create(
        nameof(TargetObject),
        typeof(VisualElement),
        typeof(SetFocusAction));

    public VisualElement? TargetObject
    {
        get => (VisualElement)GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

    public void Execute(BindableObject associatedObject, object? parameter)
    {
        var element = TargetObject ?? (associatedObject as VisualElement);
        element?.Focus();
    }
}
