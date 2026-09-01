namespace Smart.Maui.Animations;

#pragma warning disable CA1002
[ContentProperty("Animations")]
public sealed class SequentialAnimation : AnimationBase
{
    public List<AnimationBase> Animations { get; }

    public SequentialAnimation()
    {
        Animations = [];
    }

    public SequentialAnimation(List<AnimationBase> animations)
    {
        Animations = animations;
    }

    protected override async Task BeginAnimation(VisualElement target)
    {
        foreach (var animation in Animations)
        {
            animation.Target ??= target;

            await animation.Begin().ConfigureAwait(true);
        }
    }
}
#pragma warning restore CA1002
