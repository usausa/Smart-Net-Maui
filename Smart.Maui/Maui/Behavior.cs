namespace Smart.Maui;

public static class Behavior
{
    public static readonly BindableProperty KeyProperty = BindableProperty.CreateAttached(
        "Key",
        typeof(string),
        typeof(Behavior),
        null);

    public static string? GetKey(BindableObject view) => (string?)view.GetValue(KeyProperty);

    public static void SetKey(BindableObject view, string? value) => view.SetValue(KeyProperty, value);
}
