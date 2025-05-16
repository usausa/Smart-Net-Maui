namespace Smart.Maui.Interactivity;

public sealed class GoToStateAction : BindableObject, IAction
{
    public static readonly BindableProperty StateNameProperty = BindableProperty.Create(
        nameof(StateName),
        typeof(string),
        typeof(GoToStateAction),
        string.Empty);

    public static readonly BindableProperty TargetObjectProperty = BindableProperty.Create(
        nameof(TargetObject),
        typeof(VisualElement),
        typeof(GoToStateAction));

    public string StateName
    {
        get => (string)GetValue(StateNameProperty);
        set => SetValue(StateNameProperty, value);
    }

    public VisualElement? TargetObject
    {
        get => (VisualElement)GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

    public void Execute(BindableObject associatedObject, object? parameter)
    {
        if (String.IsNullOrEmpty(StateName))
        {
            return;
        }

        var element = TargetObject ?? (associatedObject as VisualElement);
        if (element is not null)
        {
            VisualStateManager.GoToState(element, StateName);
        }
    }
}
