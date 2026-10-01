namespace Smart.Maui.Interactivity;

using Smart.Maui.Internal;

public sealed class PressEffectBehavior : BehaviorBase<View>
{
    public static readonly BindableProperty PressedScaleProperty = BindableProperty.Create(
        nameof(PressedScale),
        typeof(double),
        typeof(PressEffectBehavior),
        0.9d);

    public static readonly BindableProperty PressedOpacityProperty = BindableProperty.Create(
        nameof(PressedOpacity),
        typeof(double),
        typeof(PressEffectBehavior),
        0.8d);

    public static readonly BindableProperty PressedDurationProperty = BindableProperty.Create(
        nameof(PressedDuration),
        typeof(uint),
        typeof(PressEffectBehavior),
        50u);

    public static readonly BindableProperty ReleasedDurationProperty = BindableProperty.Create(
        nameof(ReleasedDuration),
        typeof(uint),
        typeof(PressEffectBehavior),
        100u);

    public static readonly BindableProperty EasingProperty = BindableProperty.Create(
        nameof(Easing),
        typeof(Easing),
        typeof(PressEffectBehavior),
        Easing.CubicOut);

    public double PressedScale
    {
        get => (double)GetValue(PressedScaleProperty);
        set => SetValue(PressedScaleProperty, value);
    }

    public double PressedOpacity
    {
        get => (double)GetValue(PressedOpacityProperty);
        set => SetValue(PressedOpacityProperty, value);
    }

    public uint PressedDuration
    {
        get => (uint)GetValue(PressedDurationProperty);
        set => SetValue(PressedDurationProperty, value);
    }

    public uint ReleasedDuration
    {
        get => (uint)GetValue(ReleasedDurationProperty);
        set => SetValue(ReleasedDurationProperty, value);
    }

    public Easing Easing
    {
        get => (Easing)GetValue(EasingProperty);
        set => SetValue(EasingProperty, value);
    }

    protected override void OnAttachedTo(View bindable)
    {
        base.OnAttachedTo(bindable);

        PressEvents.Add(bindable, OnPressed, OnReleased);
    }

    protected override void OnDetachingFrom(View bindable)
    {
        PressEvents.Remove(bindable, OnPressed, OnReleased);

        base.OnDetachingFrom(bindable);
    }

    private void OnPressed(object? sender, EventArgs e)
    {
        if (sender is View view)
        {
            _ = view.ScaleToAsync(PressedScale, PressedDuration, Easing);
            _ = view.FadeToAsync(PressedOpacity, PressedDuration, Easing);
        }
    }

    private void OnReleased(object? sender, EventArgs e)
    {
        if (sender is View view)
        {
            _ = view.ScaleToAsync(1, ReleasedDuration, Easing);
            _ = view.FadeToAsync(1, ReleasedDuration, Easing);
        }
    }
}
