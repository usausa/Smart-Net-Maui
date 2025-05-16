namespace Smart.Maui.Interactivity;

using System.Reflection;

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
            (cachedMethod.DeclaringType != target.GetType()) ||
            (cachedMethod.Name != methodName))
        {
            cachedMethod = target.GetType().GetRuntimeMethods().FirstOrDefault(m =>
                m.Name == methodName &&
                (m.GetParameters().Length == 0));
            if (cachedMethod is null)
            {
                return;
            }
        }

        args.Result = cachedMethod.Invoke(target, null);
    }
}
