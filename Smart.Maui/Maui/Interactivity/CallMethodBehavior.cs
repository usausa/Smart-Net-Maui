namespace Smart.Maui.Interactivity;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Smart.Linq;

[RequiresUnreferencedCode("Uses reflection to subscribe to events and invoke methods by name; metadata may be trimmed.")]
public sealed class CallMethodBehavior : BehaviorBase<BindableObject>
{
    public static readonly BindableProperty EventNameProperty = BindableProperty.Create(
        nameof(EventName),
        typeof(string),
        typeof(CallMethodBehavior),
        string.Empty,
        propertyChanged: HandleEventNamePropertyChanged);

    public static readonly BindableProperty TargetObjectProperty = BindableProperty.Create(
        nameof(TargetObject),
        typeof(object),
        typeof(CallMethodBehavior));

    public static readonly BindableProperty MethodNameProperty = BindableProperty.Create(
        nameof(MethodName),
        typeof(string),
        typeof(CallMethodBehavior),
        string.Empty);

    public static readonly BindableProperty MethodParameterProperty = BindableProperty.Create(
        nameof(MethodParameter),
        typeof(object),
        typeof(CallMethodBehavior));

    private static readonly MethodInfo OnEventMethod = typeof(CallMethodBehavior).GetTypeInfo().GetDeclaredMethod(nameof(OnEvent))!;

    private EventInfo? eventInfo;

    private Delegate? handler;

    private MethodInvoker? cachedInvoker;

    private string cachedMethodName = string.Empty;

    private Type? cachedType;

    private Type? cachedParameterType;

    private bool cachedHasParameter;

    public string EventName
    {
        get => (string)GetValue(EventNameProperty);
        set => SetValue(EventNameProperty, value);
    }

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

    protected override void OnAttachedTo(BindableObject bindable)
    {
        base.OnAttachedTo(bindable);

        AddEventHandler(EventName);
    }

    protected override void OnDetachingFrom(BindableObject bindable)
    {
        RemoveEventHandler();

        base.OnDetachingFrom(bindable);
    }

    private void AddEventHandler(string eventName)
    {
        if (String.IsNullOrEmpty(eventName))
        {
            return;
        }

        eventInfo = AssociatedObject!.GetType().GetRuntimeEvent(EventName);
        if (eventInfo is null)
        {
            throw new ArgumentException(nameof(EventName));
        }

        handler = OnEventMethod.CreateDelegate(eventInfo.EventHandlerType!, this);
        eventInfo.AddEventHandler(AssociatedObject, handler);
    }

    private void RemoveEventHandler()
    {
        eventInfo?.RemoveEventHandler(AssociatedObject, handler);
        eventInfo = null;
        handler = null;
    }

    // ReSharper disable UnusedParameter.Local
    private void OnEvent(object sender, EventArgs e)
    {
        var target = TargetObject ?? BindingContext;
        var methodName = MethodName;
        if ((target is null) || String.IsNullOrEmpty(methodName))
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
            cachedInvoker.Invoke(target, methodParameter);
        }
        else
        {
            cachedInvoker.Invoke(target);
        }
    }
    // ReSharper restore UnusedParameter.Local

    private static void HandleEventNamePropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        var behavior = (CallMethodBehavior)bindable;
        if (behavior.AssociatedObject is null)
        {
            return;
        }

        behavior.RemoveEventHandler();
        if (newValue is not null)
        {
            behavior.AddEventHandler((string)newValue);
        }
    }
}
