namespace Smart.Maui.Animations;

using System.Globalization;

public enum FadeDirection
{
    Up,
    Down
}

public sealed class FadeToAnimation : AnimationBase
{
    public static readonly BindableProperty OpacityProperty = BindableProperty.Create(
        nameof(Opacity),
        typeof(double),
        typeof(FadeToAnimation),
        0.0d,
        BindingMode.TwoWay);

    public double Opacity
    {
        get => (double)GetValue(OpacityProperty);
        set => SetValue(OpacityProperty, value);
    }

    protected override Task BeginAnimation(VisualElement target)
    {
        return target.FadeTo(Opacity, Duration, Easing);
    }
}

public sealed class FadeInAnimation : AnimationBase
{
    public static readonly BindableProperty DirectionProperty = BindableProperty.Create(
        nameof(Direction),
        typeof(FadeDirection),
        typeof(FadeInAnimation),
        FadeDirection.Up,
        BindingMode.TwoWay);

    public FadeDirection Direction
    {
        get => (FadeDirection)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    protected override Task BeginAnimation(VisualElement target)
    {
        return Task.Run(() => target.Dispatcher.Dispatch(() =>
            Target.Animate("FadeIn", FadeIn(), 16, Duration)));
    }

    private Animation FadeIn()
    {
        var animation = new Animation();
        animation.WithConcurrent(
            x => Target!.Opacity = x,
            0,
            1,
            Easing.CubicOut);
        animation.WithConcurrent(
            x => Target!.TranslationY = x,
            Target!.TranslationY + ((Direction == FadeDirection.Up) ? 50 : -50),
            Target!.TranslationY,
            Easing.CubicOut);
        return animation;
    }
}

public sealed class FadeOutAnimation : AnimationBase
{
    public static readonly BindableProperty DirectionProperty = BindableProperty.Create(
        nameof(Direction),
        typeof(FadeDirection),
        typeof(FadeOutAnimation),
        FadeDirection.Up,
        BindingMode.TwoWay);

    public FadeDirection Direction
    {
        get => (FadeDirection)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    protected override Task BeginAnimation(VisualElement target)
    {
        return Task.Run(() => target.Dispatcher.Dispatch(() =>
            Target.Animate("FadeOut", FadeOut(), 16, Duration)));
    }

    private Animation FadeOut()
    {
        var animation = new Animation();
        animation.WithConcurrent(
            x => Target!.Opacity = x,
            1,
            0);
        animation.WithConcurrent(
            x => Target!.TranslationY = x,
            Target!.TranslationY,
            Target!.TranslationY + ((Direction == FadeDirection.Up) ? 50 : -50));
        return animation;
    }
}
