namespace Smart.Maui.Interactivity;

using Smart.Maui.Internal;

public sealed class HapticFeedbackBehavior : BehaviorBase<View>
{
    public static readonly BindableProperty FeedbackTypeProperty = BindableProperty.Create(
        nameof(FeedbackType),
        typeof(HapticFeedbackType),
        typeof(HapticFeedbackBehavior),
        HapticFeedbackType.Click);

    public HapticFeedbackType FeedbackType
    {
        get => (HapticFeedbackType)GetValue(FeedbackTypeProperty);
        set => SetValue(FeedbackTypeProperty, value);
    }

    protected override void OnAttachedTo(View bindable)
    {
        base.OnAttachedTo(bindable);

        PressEvents.Add(bindable, OnPressed, null);
    }

    protected override void OnDetachingFrom(View bindable)
    {
        PressEvents.Remove(bindable, OnPressed, null);

        base.OnDetachingFrom(bindable);
    }

    private void OnPressed(object? sender, EventArgs e)
    {
        try
        {
            HapticFeedback.Default.Perform(FeedbackType);
        }
        catch (FeatureNotSupportedException)
        {
            // Ignore
        }
    }
}
