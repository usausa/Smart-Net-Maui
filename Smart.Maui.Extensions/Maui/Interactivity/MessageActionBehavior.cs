namespace Smart.Maui.Interactivity;

using Smart.Mvvm.Messaging;

public sealed class MessageActionBehavior : ActionBehaviorBase<VisualElement>
{
    public static readonly BindableProperty MessengerProperty = BindableProperty.Create(
        nameof(Messenger),
        typeof(IMessenger),
        typeof(MessageActionBehavior),
        propertyChanged: HandleMessengerPropertyChanged);

    public IMessenger? Messenger
    {
        get => (IMessenger)GetValue(MessengerProperty);
        set => SetValue(MessengerProperty, value);
    }

    public string? Label { get; set; }

    public Type? MessageType { get; set; }

    private bool subscribed;

    protected override void OnAttachedTo(VisualElement bindable)
    {
        base.OnAttachedTo(bindable);

        bindable.Loaded += OnLoaded;
        bindable.Unloaded += OnUnloaded;

        if (bindable.IsLoaded)
        {
            Subscribe();
        }
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        Unsubscribe();

        bindable.Loaded -= OnLoaded;
        bindable.Unloaded -= OnUnloaded;

        base.OnDetachingFrom(bindable);
    }

    private void OnLoaded(object? sender, EventArgs e) => Subscribe();

    private void OnUnloaded(object? sender, EventArgs e) => Unsubscribe();

    private void Subscribe()
    {
        if (subscribed)
        {
            return;
        }

        subscribed = true;
        AddReceived(Messenger);
    }

    private void Unsubscribe()
    {
        if (!subscribed)
        {
            return;
        }

        subscribed = false;
        RemoveReceived(Messenger);
    }

    private void AddReceived(IMessenger? messenger)
    {
        if (messenger is not null)
        {
            messenger.Received += MessengerOnReceived;
        }
    }

    private void RemoveReceived(IMessenger? messenger)
    {
        if (messenger is not null)
        {
            messenger.Received -= MessengerOnReceived;
        }
    }

    private static void HandleMessengerPropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (oldValue == newValue)
        {
            return;
        }

        var behavior = (MessageActionBehavior)bindable;
        if (!behavior.subscribed)
        {
            return;
        }

        behavior.RemoveReceived(oldValue as IMessenger);
        behavior.AddReceived(newValue as IMessenger);
    }

    private void MessengerOnReceived(object? sender, MessengerEventArgs e)
    {
        var label = Label;
        var messageType = MessageType;
        if (((label is null) || label.Equals(e.Label, StringComparison.Ordinal)) &&
            ((messageType is null) || ((e.MessageType is not null) && messageType.IsAssignableFrom(e.MessageType))))
        {
            InvokeActions(e.Message);
        }
    }
}
