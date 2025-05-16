namespace Smart.Maui.Interactivity;

using System.Reflection;

using Smart.Mvvm.Messaging;

public sealed class ResolvePropertyAction : BindableObject, IAction
{
    public static readonly BindableProperty TargetObjectProperty = BindableProperty.Create(
        nameof(TargetObject),
        typeof(object),
        typeof(ResolvePropertyAction));

    public static readonly BindableProperty PropertyNameProperty = BindableProperty.Create(
        nameof(PropertyName),
        typeof(string),
        typeof(ResolvePropertyAction),
        string.Empty);

    public object? TargetObject
    {
        get => GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

    public string PropertyName
    {
        get => (string)GetValue(PropertyNameProperty);
        set => SetValue(PropertyNameProperty, value);
    }

    private PropertyInfo? cachedProperty;

    public void Execute(BindableObject associatedObject, object? parameter)
    {
        if (parameter is not ResolveEventArgs args)
        {
            return;
        }

        var target = TargetObject ?? associatedObject;
        var propertyName = PropertyName;
        if (String.IsNullOrEmpty(propertyName))
        {
            return;
        }

        if ((cachedProperty is null) ||
            (cachedProperty.DeclaringType != target.GetType()) ||
            (cachedProperty.Name != propertyName))
        {
            cachedProperty = target.GetType().GetRuntimeProperty(propertyName);
            if (cachedProperty is null)
            {
                return;
            }
        }

        args.Result = cachedProperty.GetValue(target);
    }
}
