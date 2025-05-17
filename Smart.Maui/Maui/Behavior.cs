namespace Smart.Maui;

public static class Behavior
{
    public static readonly BindableProperty KeyProperty = BindableProperty.CreateAttached(
        "Key",
        typeof(string),
        typeof(Behavior),
        null);

    public static string? GetKey(BindableObject obj) => (string?)obj.GetValue(KeyProperty);

    public static void SetKey(BindableObject obj, string? value) => obj.SetValue(KeyProperty, value);
}
