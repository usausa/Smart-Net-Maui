namespace Smart.Maui.Interactivity;

using System.Reflection;

public sealed class EventActionBehavior : ActionBehaviorBase<BindableObject>
{
    public static readonly BindableProperty EventNameProperty = BindableProperty.Create(
        nameof(EventName),
        typeof(string),
        typeof(EventActionBehavior),
        string.Empty,
        propertyChanged: HandleEventNamePropertyChanged);

    private EventInfo? eventInfo;

    private Delegate? handler;

    public string EventName
    {
        get => (string)GetValue(EventNameProperty);
        set => SetValue(EventNameProperty, value);
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

        eventInfo = AssociatedObject?.GetType().GetRuntimeEvent(EventName);
        if (eventInfo is null)
        {
            throw new ArgumentException(nameof(EventName));
        }

        var methodInfo = typeof(EventActionBehavior).GetTypeInfo().GetDeclaredMethod(nameof(OnEvent))!;
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
        InvokeActions(e);
    }
    // ReSharper restore UnusedParameter.Local

    private static void HandleEventNamePropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        var behavior = (EventActionBehavior)bindable;
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
