namespace Smart.Maui.Interactivity;

public sealed class TapActionBehavior : ActionBehaviorBase<View>
{
    public static readonly BindableProperty ParameterProperty = BindableProperty.Create(
        nameof(Parameter),
        typeof(object),
        typeof(TimerActionBehavior));

    public object Parameter
    {
        get => GetValue(ParameterProperty);
        set => SetValue(ParameterProperty, value);
    }

    private TapGestureRecognizer? gesture;

    protected override void OnAttachedTo(View bindable)
    {
        base.OnAttachedTo(bindable);

        gesture = new TapGestureRecognizer();
        gesture.Tapped += OnTapped;
        bindable.GestureRecognizers.Add(gesture);
    }

    protected override void OnDetachingFrom(View bindable)
    {
        base.OnDetachingFrom(bindable);
        if (gesture is not null)
        {
            gesture.Tapped -= OnTapped;
            bindable.GestureRecognizers.Remove(gesture);
            gesture = null;
        }
    }

    private void OnTapped(object? sender, EventArgs e)
    {
        InvokeActions(Parameter);
    }
}
