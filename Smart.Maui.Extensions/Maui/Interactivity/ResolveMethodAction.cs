namespace Smart.Maui.Interactivity;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Smart.Linq;
using Smart.Mvvm.Messaging;

[RequiresUnreferencedCode("Uses GetRuntimeMethods to find method by name; method metadata may be trimmed.")]
public sealed class ResolveMethodAction : BindableObject, IAction
{
    public static readonly BindableProperty TargetObjectProperty = BindableProperty.Create(
        nameof(TargetObject),
        typeof(object),
        typeof(ResolveMethodAction));

    public static readonly BindableProperty MethodNameProperty = BindableProperty.Create(
        nameof(MethodName),
        typeof(string),
        typeof(ResolveMethodAction),
        string.Empty);

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

    private MethodInvoker? cachedInvoker;

    private string cachedMethodName = string.Empty;

    private Type? cachedType;

    public void Execute(BindableObject associatedObject, object? parameter)
    {
        if (parameter is not ResolveEventArgs args)
        {
            return;
        }

        var target = TargetObject ?? associatedObject;
        var methodName = MethodName;
        if (String.IsNullOrEmpty(methodName))
        {
            return;
        }

        if ((cachedInvoker is null) ||
            (cachedType != target.GetType()) ||
            (cachedMethodName != methodName))
        {
            var methodInfo = target.GetType().GetRuntimeMethods()
                .FirstOrDefault(methodName, static (m, s) => (m.Name == s) && (m.GetParameters().Length == 0));
            if (methodInfo is null)
            {
                return;
            }

            cachedInvoker = MethodInvoker.Create(methodInfo);
            cachedMethodName = methodName;
            cachedType = target.GetType();
        }

        args.Result = cachedInvoker.Invoke(target);
    }
}
