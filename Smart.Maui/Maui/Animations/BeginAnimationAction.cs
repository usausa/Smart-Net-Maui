namespace Smart.Maui.Animations;

using Smart.Maui.Interactivity;

[ContentProperty("Animation")]
public sealed class BeginAnimationAction : BindableObject, IAction
{
    public static readonly BindableProperty AnimationProperty = BindableProperty.Create(
        nameof(Animation),
        typeof(AnimationBase),
        typeof(BeginAnimationBehavior));

    public AnimationBase? Animation
    {
        get => (AnimationBase)GetValue(AnimationProperty);
        set => SetValue(AnimationProperty, value);
    }

    // ReSharper disable once AsyncVoidMethod
    public async void Execute(BindableObject associatedObject, object? parameter)
    {
        if ((associatedObject is VisualElement visual) && (Animation is not null))
        {
            Animation.Target ??= visual;

            await Animation.Begin();
        }
    }
}
