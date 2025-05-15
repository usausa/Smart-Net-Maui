namespace Smart.Maui.Validation;

using Microsoft.Maui.Controls;

public static class ValidationProperty
{
    // ------------------------------------------------------------
    // ClearErrorOnFocus
    // ------------------------------------------------------------

    public static readonly BindableProperty ClearErrorOnFocusProperty = BindableProperty.CreateAttached(
        "ClearErrorOnFocus",
        typeof(bool),
        typeof(ValidationProperty),
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
        if (oldValue == newValue)
        {
            return;
        }

        if (bindable is not VisualElement visual)
        {
            return;
        }

        var behavior = visual.Behaviors.FirstOrDefault(static x => x is ClearErrorOnFocusBehavior);
        if ((oldValue is true) && (behavior is not null))
        {
            visual.Behaviors.Remove(behavior);
        }
        if ((newValue is true) && (behavior is null))
        {
            visual.Behaviors.Add(new ClearErrorOnFocusBehavior());
        }
    }

    // ------------------------------------------------------------
    // ValidateOnUnfocused
    // ------------------------------------------------------------

    public static readonly BindableProperty ValidateOnUnfocusedProperty = BindableProperty.CreateAttached(
        "ValidateOnUnfocused",
        typeof(bool),
        typeof(ValidationProperty),
        false,
        propertyChanged: HandleValidateOnUnfocusedChanged);

    public static bool GetValidateOnUnfocused(BindableObject obj)
    {
        return (bool)obj.GetValue(ValidateOnUnfocusedProperty);
    }

    public static void SetValidateOnUnfocused(BindableObject obj, bool value)
    {
        obj.SetValue(ValidateOnUnfocusedProperty, value);
    }

    private static void HandleValidateOnUnfocusedChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (oldValue == newValue)
        {
            return;
        }

        if (bindable is not VisualElement visual)
        {
            return;
        }

        var behavior = visual.Behaviors.FirstOrDefault(static x => x is ValidateOnUnfocusedBehavior);
        if ((oldValue is true) && (behavior is not null))
        {
            visual.Behaviors.Remove(behavior);
        }
        if ((newValue is true) && (behavior is null))
        {
            visual.Behaviors.Add(new ValidateOnUnfocusedBehavior());
        }
    }

    // ------------------------------------------------------------
    // ValidateOnTextChanged
    // ------------------------------------------------------------

    public static readonly BindableProperty ValidateOnTextChangedProperty = BindableProperty.CreateAttached(
        "ValidateOnTextChanged",
        typeof(bool),
        typeof(ValidationProperty),
        false,
        propertyChanged: HandleValidateOnTextChangedChanged);

    public static bool GetValidateOnTextChanged(BindableObject obj)
    {
        return (bool)obj.GetValue(ValidateOnTextChangedProperty);
    }

    public static void SetValidateOnTextChanged(BindableObject obj, bool value)
    {
        obj.SetValue(ValidateOnTextChangedProperty, value);
    }

    private static void HandleValidateOnTextChangedChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (oldValue == newValue)
        {
            return;
        }

        if (bindable is not VisualElement visual)
        {
            return;
        }

        var behavior = visual.Behaviors.FirstOrDefault(static x => x is ValidateOnTextChangedBehavior);
        if ((oldValue is true) && (behavior is not null))
        {
            visual.Behaviors.Remove(behavior);
        }
        if ((newValue is true) && (behavior is null))
        {
            visual.Behaviors.Add(new ValidateOnTextChangedBehavior());
        }
    }
}
