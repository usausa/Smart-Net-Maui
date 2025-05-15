namespace Smart.Maui.Validation;

public static class Validation
{
    // ------------------------------------------------------------
    // ClearErrorOnFocus
    // ------------------------------------------------------------

    public static readonly BindableProperty ClearErrorOnFocusProperty = BindableProperty.CreateAttached(
        "ClearErrorOnFocus",
        typeof(bool),
        typeof(Validation),
        false,
        propertyChanged: HandleClearErrorOnFocusChanged);

    public static bool GetClearErrorOnFocus(BindableObject obj)
    {
        return (bool)obj.GetValue(ClearErrorOnFocusProperty);
    }

    public static void SetClearErrorOnFocus(BindableObject obj, bool value)
    {
        obj.SetValue(ClearErrorOnFocusProperty, value);
    }

    private static void HandleClearErrorOnFocusChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
    }

}
