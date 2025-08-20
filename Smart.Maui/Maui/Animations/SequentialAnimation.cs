namespace Smart.Maui.Animations;

[ContentProperty("Animations")]
public sealed class SequentialAnimation : AnimationBase
{
#pragma warning disable CA1002
    public List<AnimationBase> Animations { get; set; }
#pragma warning restore CA1002

    public SequentialAnimation()
    {
        Animations = [];
    }

#pragma warning disable CA1002
    public SequentialAnimation(List<AnimationBase> animations)
    {
        Animations = animations;
    }
#pragma warning restore CA1002

    protected override async Task BeginAnimation(VisualElement target)
    {
        foreach (var animation in Animations)
        {
            animation.Target ??= target;

            await animation.Begin();
        }
    }
}
