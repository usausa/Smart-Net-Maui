namespace Smart.Maui.Animations;

[ContentProperty("Animations")]
public sealed class ParallelAnimation : AnimationBase
{
#pragma warning disable CA1002
    public List<AnimationBase> Animations { get; set; }
#pragma warning restore CA1002

    public ParallelAnimation()
    {
        Animations = [];
    }

#pragma warning disable CA1002
    public ParallelAnimation(List<AnimationBase> animations)
    {
        Animations = animations;
    }
#pragma warning restore CA1002

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
