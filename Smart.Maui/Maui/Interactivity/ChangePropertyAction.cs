namespace Smart.Maui.Interactivity;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

public sealed class ChangePropertyAction : BindableObject, IAction
{
    public static readonly BindableProperty TargetObjectProperty = BindableProperty.Create(
        nameof(TargetObject),
        typeof(object),
        typeof(ChangePropertyAction));

    public static readonly BindableProperty PropertyNameProperty = BindableProperty.Create(
        nameof(PropertyName),
        typeof(string),
        typeof(ChangePropertyAction),
        string.Empty);

    public static readonly BindableProperty ParameterProperty = BindableProperty.Create(
        nameof(Parameter),
        typeof(object),
        typeof(ChangePropertyAction));

    public static readonly BindableProperty ConverterProperty = BindableProperty.Create(
        nameof(Converter),
        typeof(IValueConverter),
        typeof(ChangePropertyAction));

    public static readonly BindableProperty ConverterParameterProperty = BindableProperty.Create(
        nameof(ConverterParameter),
        typeof(object),
        typeof(ChangePropertyAction));

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

    public object? Parameter
    {
        get => GetValue(ParameterProperty);
        set => SetValue(ParameterProperty, value);
    }

    public IValueConverter? Converter
    {
        get => (IValueConverter)GetValue(ConverterProperty);
        set => SetValue(ConverterProperty, value);
    }

    public object ConverterParameter
    {
        get => GetValue(ConverterParameterProperty);
        set => SetValue(ConverterParameterProperty, value);
    }

    private PropertyInfo? cachedProperty;

    private Type? cachedType;

    [RequiresUnreferencedCode("Uses GetRuntimeProperty to find property by name; property metadata may be trimmed.")]
    public ChangePropertyAction()
    {
    }

    [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Target type is determined at runtime via XAML; callers must ensure the type is preserved")]
    public void Execute(BindableObject associatedObject, object? parameter)
    {
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

        var value = Parameter;
        var converter = Converter;
        var propertyValue = (value is not null) || IsSet(ParameterProperty)
            ? value
            : converter is not null ? converter.Convert(parameter, typeof(object), ConverterParameter, CultureInfo.CurrentCulture) : parameter;
        cachedProperty.SetValue(target, propertyValue);
    }
}
