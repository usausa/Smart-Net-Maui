namespace Smart.Maui.Animations;

public sealed class RotateToAnimation : AnimationBase
{
    public static readonly BindableProperty RotationProperty = BindableProperty.Create(
        nameof(Rotation),
        typeof(double),
        typeof(RotateToAnimation),
        0.0d,
        BindingMode.TwoWay);

    public double Rotation
    {
        get => (double)GetValue(RotationProperty);
        set => SetValue(RotationProperty, value);
    }

    protected override Task BeginAnimation(VisualElement target)
    {
        return target.RotateTo(Rotation, Duration, Easing);
    }
}

public sealed class RelRotateToAnimation : AnimationBase
{
    public static readonly BindableProperty RotationProperty = BindableProperty.Create(
        nameof(Rotation),
        typeof(double),
        typeof(RelRotateToAnimation),
        0.0d,
        BindingMode.TwoWay);

    public double Rotation
    {
        get => (double)GetValue(RotationProperty);
        set => SetValue(RotationProperty, value);
    }

    protected override Task BeginAnimation(VisualElement target)
    {
        return target.RelRotateTo(Rotation, Duration, Easing);
    }
}

public sealed class RotateXToAnimation : AnimationBase
{
    public static readonly BindableProperty RotationProperty = BindableProperty.Create(
        nameof(Rotation),
        typeof(double),
        typeof(RotateXToAnimation),
        0.0d,
        BindingMode.TwoWay);

    public double Rotation
    {
        get => (double)GetValue(RotationProperty);
        set => SetValue(RotationProperty, value);
    }

    protected override Task BeginAnimation(VisualElement target)
    {
        return target.RotateXTo(Rotation, Duration, Easing);
    }
}

public sealed class RotateYToAnimation : AnimationBase
{
    public static readonly BindableProperty RotationProperty = BindableProperty.Create(
        nameof(Rotation),
        typeof(double),
        typeof(RotateYToAnimation),
        0.0d,
        BindingMode.TwoWay);

    public double Rotation
    {
        get => (double)GetValue(RotationProperty);
        set => SetValue(RotationProperty, value);
    }

    protected override Task BeginAnimation(VisualElement target)
    {
        return target.RotateYTo(Rotation, Duration, Easing);
    }
}
