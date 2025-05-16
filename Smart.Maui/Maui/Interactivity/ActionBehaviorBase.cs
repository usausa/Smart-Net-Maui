namespace Smart.Maui.Interactivity;

[ContentProperty("Actions")]
public abstract class ActionBehaviorBase<TBindable> : BehaviorBase<TBindable>
    where TBindable : BindableObject
{
    public IList<IAction> Actions { get; } = [];

    protected void InvokeActions(object? parameter)
    {
        if (AssociatedObject is null)
        {
            return;
        }

        foreach (var action in Actions)
        {
            action.Execute(AssociatedObject, parameter);
        }
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        foreach (var action in Actions)
        {
            if (action is BindableObject bindable)
            {
                bindable.BindingContext = BindingContext;
            }
        }
    }
}
