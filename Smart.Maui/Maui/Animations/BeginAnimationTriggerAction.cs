namespace Smart.Maui.Animations;

[ContentProperty("Animation")]
public sealed class BeginAnimationTriggerAction : TriggerAction<VisualElement>
{
    public AnimationBase? Animation { get; set; }

    protected override async void Invoke(VisualElement sender)
    {
        if (Animation is not null)
        {
            await Animation.Begin();
        }
    }
}
