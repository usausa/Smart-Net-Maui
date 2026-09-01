namespace Smart.Maui.Animations;

#pragma warning disable CA1002
[ContentProperty("Animations")]
public sealed class ParallelAnimation : AnimationBase
{
    public List<AnimationBase> Animations { get; }

    public ParallelAnimation()
    {
        Animations = [];
    }

    public ParallelAnimation(List<AnimationBase> animations)
    {
        Animations = animations;
    }

    protected override Task BeginAnimation(VisualElement target)
    {
        var tasks = new List<Task>(Animations.Count);
        foreach (var animation in Animations)
        {
            animation.Target ??= target;

            tasks.Add(animation.Begin());
        }

        return Task.WhenAll(tasks);
    }
}
#pragma warning restore CA1002
