namespace Smart.Maui.Interactivity;

public class DefaultFocusBehavior : BehaviorBase<VisualElement>
{
    protected override void OnAttachedTo(VisualElement bindable)
    {
        base.OnAttachedTo(bindable);

        bindable.Dispatcher.Dispatch(() => bindable.Focus());
    }
}
