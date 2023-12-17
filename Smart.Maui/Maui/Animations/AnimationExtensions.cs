namespace Smart.Maui.Animations;

public static class AnimationExtensions
{
    public static async Task<bool> Animate(this VisualElement visualElement, AnimationBase animation)
    {
#pragma warning disable CA1031
        try
        {
            animation.Target = visualElement;

            await animation.Begin();

            return true;
        }
        catch
        {
            return false;
        }
#pragma warning restore CA1031
    }

    public static Task<bool> ColorTo(this VisualElement self, Color fromColor, Color toColor, Action<Color> callback, uint length = 250, Easing? easing = null)
    {
        Color Transform(double x) => Color.FromRgba(
            fromColor.Red + (x * (toColor.Red - fromColor.Red)),
            fromColor.Green + (x * (toColor.Green - fromColor.Green)),
            fromColor.Blue + (x * (toColor.Blue - fromColor.Blue)),
            fromColor.Alpha + (x * (toColor.Alpha - fromColor.Alpha)));
        return ColorAnimation(self, "ColorTo", Transform, callback, length, easing);
    }

    private static Task<bool> ColorAnimation(VisualElement element, string name, Func<double, Color> transform, Action<Color> callback, uint length, Easing? easing)
    {
        var tcs = new TaskCompletionSource<bool>();
        element.Animate(name, transform, callback, 16, length, easing ?? Easing.Linear, (_, c) => tcs.SetResult(c));
        return tcs.Task;
    }
}
