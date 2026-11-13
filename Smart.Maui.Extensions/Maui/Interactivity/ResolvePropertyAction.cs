namespace Smart.Maui.Interactivity;

using System.Diagnostics.CodeAnalysis;
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

    private Type? cachedType;

    [RequiresUnreferencedCode("Uses GetRuntimeProperty to find property by name; property metadata may be trimmed.")]
    public ResolvePropertyAction()
    {
    }

    [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Target type is determined at runtime via XAML; callers must ensure the type is preserved")]
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
            (cachedType != target.GetType()) ||
            (cachedProperty.Name != propertyName))
        {
            cachedProperty = target.GetType().GetRuntimeProperty(propertyName);
            cachedType = target.GetType();
            if (cachedProperty is null)
            {
                return;
            }
        }

        args.Result = cachedProperty.GetValue(target);
    }
}
