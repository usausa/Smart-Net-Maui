namespace Smart.Maui.Animations;

using System.Globalization;

public sealed class ScaleToAnimation : AnimationBase
{
    public static readonly BindableProperty ScaleProperty = BindableProperty.Create(
        nameof(Scale),
        typeof(double),
        typeof(ScaleToAnimation),
        0.0d,
        BindingMode.TwoWay);

    public double Scale
    {
        get => (double)GetValue(ScaleProperty);
        set => SetValue(ScaleProperty, value);
    }

    protected override Task BeginAnimation(VisualElement target)
    {
        return target.ScaleTo(Scale, Duration, Easing);
    }
}

public sealed class RelScaleToAnimation : AnimationBase
{
    public static readonly BindableProperty ScaleProperty = BindableProperty.Create(
        nameof(Scale),
        typeof(double),
        typeof(RelScaleToAnimation),
        0.0d,
        BindingMode.TwoWay);

    public double Scale
    {
        get => (double)GetValue(ScaleProperty);
        set => SetValue(ScaleProperty, value);
    }

    protected override Task BeginAnimation(VisualElement target)
    {
        return target.RelScaleTo(Scale, Duration, Easing);
    }
}
