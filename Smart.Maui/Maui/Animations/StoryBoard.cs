namespace Smart.Maui.Animations;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1002:DoNotExposeGenericLists", Justification = "Ignore")]
[ContentProperty("Animations")]
public sealed class StoryBoard : AnimationBase
{
    public List<AnimationBase> Animations { get; }

    public StoryBoard()
    {
        Animations = [];
    }

    public StoryBoard(List<AnimationBase> animations)
    {
        Animations = animations;
    }

    protected override async Task BeginAnimation(VisualElement target)
    {
        foreach (var animation in Animations)
        {
            animation.Target ??= target;

            await animation.Begin();
        }
    }
}
