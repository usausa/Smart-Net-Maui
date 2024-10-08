namespace Smart.Maui.Interactivity;

using System.Reflection;

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

    private EventInfo? eventInfo;

    private Delegate? handler;

    private MethodInfo? cachedMethod;

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

        var methodInfo = typeof(CallMethodBehavior).GetTypeInfo().GetDeclaredMethod(nameof(OnEvent))!;
        handler = methodInfo.CreateDelegate(eventInfo.EventHandlerType!, this);
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
        if ((target is null) || string.IsNullOrEmpty(methodName))
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

        cachedMethod.Invoke(target, cachedMethod.GetParameters().Length > 0 ? [MethodParameter] : null);
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
