namespace Smart.Maui.Interactivity;

using System.Globalization;
using System.Reflection;

public sealed class CallMethodAction : BindableObject, IAction
{
    public static readonly BindableProperty TargetObjectProperty = BindableProperty.Create(
        nameof(TargetObject),
        typeof(object),
        typeof(CallMethodAction));

    public static readonly BindableProperty MethodNameProperty = BindableProperty.Create(
        nameof(MethodName),
        typeof(string),
        typeof(CallMethodAction),
        string.Empty);

    public static readonly BindableProperty MethodParameterProperty = BindableProperty.Create(
        nameof(MethodParameter),
        typeof(object),
        typeof(CallMethodAction));

    public static readonly BindableProperty ConverterProperty = BindableProperty.Create(
        nameof(Converter),
        typeof(IValueConverter),
        typeof(CallMethodAction));

    public static readonly BindableProperty ConverterParameterProperty = BindableProperty.Create(
        nameof(ConverterParameter),
        typeof(object),
        typeof(CallMethodAction));

    public object? TargetObject
    {
        get => GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

    public string MethodName
    {
        get => (string)GetValue(MethodNameProperty);
        set => SetValue(MethodNameProperty, value);
    }

    public object? MethodParameter
    {
        get => GetValue(MethodParameterProperty);
        set => SetValue(MethodParameterProperty, value);
    }

    public IValueConverter? Converter
    {
        get => (IValueConverter)GetValue(ConverterProperty);
        set => SetValue(ConverterProperty, value);
    }

    public object? ConverterParameter
    {
        get => GetValue(ConverterParameterProperty);
        set => SetValue(ConverterParameterProperty, value);
    }

    private MethodInfo? cachedMethod;

    public void Execute(BindableObject associatedObject, object? parameter)
    {
        var target = TargetObject ?? associatedObject;
        var methodName = MethodName;
        if (String.IsNullOrEmpty(methodName))
        {
            return;
        }

        if ((cachedMethod is null) ||
            (cachedMethod.DeclaringType != target.GetType()) ||
            (cachedMethod.Name != methodName))
        {
            var methodInfo = target.GetType().GetRuntimeMethods().FirstOrDefault(m =>
                m.Name == methodName &&
                ((m.GetParameters().Length == 0) ||
                 ((m.GetParameters().Length == 1) &&
                  ((MethodParameter is null) ||
                   MethodParameter.GetType().GetTypeInfo().IsAssignableFrom(m.GetParameters()[0].ParameterType.GetTypeInfo())))));
            if (methodInfo is null)
            {
                return;
            }

            cachedMethod = methodInfo;
        }

        if (cachedMethod.GetParameters().Length > 0)
        {
            var methodParameter = MethodParameter;
            var argument = (methodParameter is not null) || IsSet(MethodParameterProperty)
                ? methodParameter
                : Converter?.Convert(parameter, typeof(object), ConverterParameter, CultureInfo.CurrentCulture) ?? parameter;
            cachedMethod.Invoke(target, [argument]);
        }
        else
        {
            cachedMethod.Invoke(target, null);
        }
    }
}
