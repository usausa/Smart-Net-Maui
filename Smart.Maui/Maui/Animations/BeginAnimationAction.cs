namespace Smart.Maui.Animations;

using Smart.Maui.Interactivity;

[ContentProperty("Animation")]
public sealed class BeginAnimationAction : ActionBase<VisualElement>
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
    protected override async void Invoke(VisualElement associatedObject, object? parameter)
    {
        if (Animation is not null)
        {
            Animation.Target ??= associatedObject;

            await Animation.Begin();
        }
    }
}
