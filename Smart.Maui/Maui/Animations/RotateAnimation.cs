namespace Smart.Maui.Animations;

using System.Globalization;

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
        return target.RotateTo(Rotation, Convert.ToUInt32(Duration, CultureInfo.InvariantCulture), Easing);
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
        return target.RelRotateTo(Rotation, Convert.ToUInt32(Duration, CultureInfo.InvariantCulture), Easing);
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
        return target.RotateXTo(Rotation, Convert.ToUInt32(Duration, CultureInfo.InvariantCulture), Easing);
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
        return target.RotateYTo(Rotation, Convert.ToUInt32(Duration, CultureInfo.InvariantCulture), Easing);
    }
}
