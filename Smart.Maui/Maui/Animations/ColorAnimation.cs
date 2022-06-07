namespace Smart.Maui.Animations;

using System.Globalization;

public sealed class ColorAnimation : AnimationBase
{
    public static readonly BindableProperty ToColorProperty = BindableProperty.Create(
        nameof(ToColor),
        typeof(Color),
        typeof(ColorAnimation),
        Colors.Transparent,
        BindingMode.TwoWay);

    public Color ToColor
    {
        get => (Color)GetValue(ToColorProperty);
        set => SetValue(ToColorProperty, value);
    }

    protected override Task BeginAnimation(VisualElement target)
    {
        var fromColor = Target!.BackgroundColor;
        // ReSharper disable once AsyncVoidLambda
        return Task.Run(() => target.Dispatcher.Dispatch(async () =>
            await target.ColorTo(fromColor, ToColor, c => target.BackgroundColor = c, Convert.ToUInt32(Duration, CultureInfo.InvariantCulture))));
    }
}
