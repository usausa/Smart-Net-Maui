namespace Smart.Maui.Interactivity;

using Smart.Maui.Messaging;

public sealed class CollectionControlBehavior : BehaviorBase<VisualElement>
{
    public static readonly BindableProperty ControllerProperty = BindableProperty.Create(
        nameof(Controller),
        typeof(CollectionController),
        typeof(CollectionControlBehavior),
        propertyChanged: HandleControllerPropertyChanged);

    public CollectionController? Controller
    {
        get => (CollectionController)GetValue(ControllerProperty);
        set => SetValue(ControllerProperty, value);
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        var controller = Controller;
        if (controller is not null)
        {
            controller.ScrollRequested -= OnScrollRequested;
        }

        base.OnDetachingFrom(bindable);
    }

    private static void HandleControllerPropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        ((CollectionControlBehavior)bindable).OnMessengerPropertyChanged(oldValue as CollectionController, newValue as CollectionController);
    }

    private void OnMessengerPropertyChanged(CollectionController? oldValue, CollectionController? newValue)
    {
        if (oldValue == newValue)
        {
            return;
        }

        if (oldValue is not null)
        {
            oldValue.ScrollRequested -= OnScrollRequested;
        }

        if (newValue is not null)
        {
            newValue.ScrollRequested += OnScrollRequested;
        }
    }

    private void OnScrollRequested(object? sender, CollectionScrollEventArgs e)
    {
        if (AssociatedObject is not CollectionView view)
        {
            return;
        }

        view.ScrollTo(e.Index, e.GroupIndex, e.Position, e.Animate);
    }
}
