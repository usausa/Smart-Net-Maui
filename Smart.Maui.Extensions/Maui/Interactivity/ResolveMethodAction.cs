namespace Smart.Maui.Interactivity;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Smart.Linq;
using Smart.Mvvm.Messaging;

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

    private MethodInfo? cachedMethod;

    private Type? cachedType;

    [RequiresUnreferencedCode("Uses GetRuntimeMethods to find method by name; method metadata may be trimmed.")]
    public ResolveMethodAction()
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
        var methodName = MethodName;
        if (String.IsNullOrEmpty(methodName))
        {
            return;
        }

        if ((cachedMethod is null) ||
            (cachedType != target.GetType()) ||
            (cachedMethod.Name != methodName))
        {
            cachedMethod = target.GetType().GetRuntimeMethods()
                .FirstOrDefault(methodName, static (m, s) => (m.Name == s) && (m.GetParameters().Length == 0));
            if (cachedMethod is null)
            {
                return;
            }

            cachedType = target.GetType();
        }

        args.Result = cachedMethod.Invoke(target, null);
    }
}
