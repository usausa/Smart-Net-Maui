namespace Smart.Maui.Validation;

using Smart.Maui.Interactivity;
using Smart.Maui.Messaging;
using Smart.Maui.ViewModels;
using Smart.Mvvm.ViewModels;

public sealed class ValidationFocusBehavior : BehaviorBase<VisualElement>
{
    public static readonly BindableProperty RequestProperty = BindableProperty.Create(
        nameof(Request),
        typeof(ValidationFocusRequest),
        typeof(ValidationFocusBehavior),
        propertyChanged: HandleRequestPropertyChanged);

    public static readonly BindableProperty TargetProperty = BindableProperty.Create(
        nameof(Target),
        typeof(ErrorInfo),
        typeof(ValidationFocusBehavior));

    public ValidationFocusRequest? Request
    {
        get => (ValidationFocusRequest)GetValue(RequestProperty);
        set => SetValue(RequestProperty, value);
    }

    public ErrorInfo? Target
    {
        get => (ErrorInfo)GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        var controller = Request;
        if (controller is not null)
        {
            controller.FocusRequested -= OnFocusRequested;
        }

        base.OnDetachingFrom(bindable);
    }

    private static void HandleRequestPropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        ((ValidationFocusBehavior)bindable).OnMessengerPropertyChanged(oldValue as ValidationFocusRequest, newValue as ValidationFocusRequest);
    }

    private void OnMessengerPropertyChanged(ValidationFocusRequest? oldValue, ValidationFocusRequest? newValue)
    {
        if (oldValue == newValue)
        {
            return;
        }

        if (oldValue is not null)
        {
            oldValue.FocusRequested -= OnFocusRequested;
        }

        if (newValue is not null)
        {
            newValue.FocusRequested += OnFocusRequested;
        }
    }

    private void OnFocusRequested(object? sender, EventArgs e)
    {
        Dispatcher.Dispatch(() =>
        {
            var root = AssociatedObject;
            if (root is null)
            {
                return;
            }

            var errorInfo = Target ?? (BindingContext is ExtendViewModelBase vm ? vm.Errors : null);
            if (errorInfo is null)
            {
                return;
            }

            var target = FindTarget(root, errorInfo);
            target?.Focus();
        });
    }

    public static VisualElement? FindTarget(VisualElement parent, ErrorInfo errorInfo)
    {
        if (!parent.IsEnabled || !parent.IsVisible)
        {
            return null;
        }

        if (parent is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                if (!child.IsEnabled || (child is not VisualElement { IsVisible: true } visual))
                {
                    continue;
                }

                var key = Behavior.GetKey(visual);
                if (!String.IsNullOrEmpty(key) && errorInfo.Contains(key))
                {
                    return visual;
                }

                var result = FindTarget(visual, errorInfo);
                if (result != null)
                {
                    return result;
                }
            }
        }

        if (parent is IContentView { Content: VisualElement visualContent })
        {
            var result = FindTarget(visualContent, errorInfo);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}
