namespace Smart.Maui.Interactivity;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

using Smart.Linq;

[RequiresUnreferencedCode("Uses GetRuntimeMethods to find method by name; method metadata may be trimmed.")]
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

    private MethodInvoker? cachedInvoker;

    private string cachedMethodName = string.Empty;

    private Type? cachedType;

    private Type? cachedParameterType;

    private bool cachedHasParameter;

    public void Execute(BindableObject associatedObject, object? parameter)
    {
        var target = TargetObject ?? associatedObject;
        var methodName = MethodName;
        if (String.IsNullOrEmpty(methodName))
        {
            return;
        }

        var methodParameter = MethodParameter;
        var parameterType = methodParameter?.GetType();
        if ((cachedInvoker is null) ||
            (cachedType != target.GetType()) ||
            (cachedMethodName != methodName) ||
            (cachedParameterType != parameterType))
        {
            var methodInfo = target.GetType().GetRuntimeMethods().FirstOrDefault((methodName, methodParameter), static (m, s) =>
                (m.Name == s.methodName) &&
                ((m.GetParameters().Length == 0) ||
                 ((m.GetParameters().Length == 1) &&
                  ((s.methodParameter is null) ||
                   m.GetParameters()[0].ParameterType.GetTypeInfo().IsAssignableFrom(s.methodParameter.GetType().GetTypeInfo())))));
            if (methodInfo is null)
            {
                return;
            }

            cachedInvoker = MethodInvoker.Create(methodInfo);
            cachedMethodName = methodName;
            cachedType = target.GetType();
            cachedParameterType = parameterType;
            cachedHasParameter = methodInfo.GetParameters().Length > 0;
        }

        if (cachedHasParameter)
        {
            var converter = Converter;
            var argument = (methodParameter is not null) || IsSet(MethodParameterProperty)
                ? methodParameter
                : converter is not null ? converter.Convert(parameter, typeof(object), ConverterParameter, CultureInfo.CurrentCulture) : parameter;
            cachedInvoker.Invoke(target, argument);
        }
        else
        {
            cachedInvoker.Invoke(target);
        }
    }
}
