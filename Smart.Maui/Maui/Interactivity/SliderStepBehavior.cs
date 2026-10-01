namespace Smart.Maui.Interactivity;

public sealed class SliderStepBehavior : BehaviorBase<Slider>
{
    public static readonly BindableProperty StepProperty = BindableProperty.Create(
        nameof(Step),
        typeof(double),
        typeof(SliderStepBehavior),
        1d);

    public double Step
    {
        get => (double)GetValue(StepProperty);
        set => SetValue(StepProperty, value);
    }

    protected override void OnAttachedTo(Slider bindable)
    {
        base.OnAttachedTo(bindable);

        bindable.ValueChanged += OnValueChanged;
    }

    protected override void OnDetachingFrom(Slider bindable)
    {
        bindable.ValueChanged -= OnValueChanged;

        base.OnDetachingFrom(bindable);
    }

    private void OnValueChanged(object? sender, ValueChangedEventArgs e)
    {
        var step = Step;
        if ((sender is Slider slider) && (step > 0))
        {
            slider.Value = slider.Minimum + (Math.Round((e.NewValue - slider.Minimum) / step) * step);
        }
    }
}
