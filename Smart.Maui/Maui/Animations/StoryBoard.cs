namespace Smart.Maui.Animations;

[ContentProperty("Animations")]
public sealed class StoryBoard : AnimationBase
{
#pragma warning disable CA1002
    public List<AnimationBase> Animations { get; }
#pragma warning restore CA1002

    public StoryBoard()
    {
        Animations = [];
    }

#pragma warning disable CA1002
    public StoryBoard(List<AnimationBase> animations)
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
