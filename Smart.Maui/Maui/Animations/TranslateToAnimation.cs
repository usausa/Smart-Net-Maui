namespace Smart.Maui.Animations;

public sealed class TranslateToAnimation : AnimationBase
{
    public static readonly BindableProperty TranslateXProperty = BindableProperty.Create(
        nameof(TranslateX),
        typeof(double),
        typeof(TranslateToAnimation),
        0.0d,
        BindingMode.TwoWay);

    public static readonly BindableProperty TranslateYProperty = BindableProperty.Create(
        nameof(TranslateY),
        typeof(double),
        typeof(TranslateToAnimation),
        0.0d,
        BindingMode.TwoWay);

    public double TranslateX
    {
        get => (double)GetValue(TranslateXProperty);
        set => SetValue(TranslateXProperty, value);
    }

    public double TranslateY
    {
        get => (double)GetValue(TranslateYProperty);
        set => SetValue(TranslateYProperty, value);
    }

    protected override Task BeginAnimation(VisualElement target)
    {
        return target.TranslateToAsync(TranslateX, TranslateY, Duration, Easing);
    }
}
