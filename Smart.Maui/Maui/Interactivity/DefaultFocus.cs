namespace Smart.Maui.Interactivity;

public static class DefaultFocus
{
    public static readonly BindableProperty IsTargetProperty = BindableProperty.CreateAttached(
        "IsTarget",
        typeof(bool),
        typeof(DefaultFocus),
        false);

    public static bool GetIsTarget(BindableObject obj) =>
        (bool)obj.GetValue(IsTargetProperty);

    public static void SetIsTarget(BindableObject obj, bool value) =>
        obj.SetValue(IsTargetProperty, value);
}
