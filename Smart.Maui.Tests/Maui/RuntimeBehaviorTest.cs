namespace Smart.Maui;

using Microsoft.Maui.Controls;

public sealed class RuntimeBehaviorTest
{
    private static RuntimeElement CreateElement() => new();

    // ------------------------------------------------------------
    // Property
    // ------------------------------------------------------------

    [Fact]
    public void PropertyIsRegistered()
    {
        // Arrange & Act
        var property = RuntimeElement.ScaleProperty;

        // Assert
        Assert.Equal(nameof(RuntimeElement.Scale), property.PropertyName);
        Assert.Equal(typeof(double), property.ReturnType);
        Assert.Equal(typeof(RuntimeElement), property.DeclaringType);
    }

    [Fact]
    public void ValueRoundTrips()
    {
        // Arrange
        var element = CreateElement();

        // Act
        element.Scale = 5d;

        // Assert
        Assert.Equal(5d, element.Scale);
        Assert.Equal(5d, element.GetValue(RuntimeElement.ScaleProperty));
    }

    // ------------------------------------------------------------
    // Default value
    // ------------------------------------------------------------

    [Fact]
    public void DefaultValueIsApplied()
    {
        // Arrange & Act
        var element = CreateElement();

        // Assert
        Assert.Equal(1d, element.Scale);
    }

    [Fact]
    public void DefaultBindingModeIsApplied()
    {
        // Arrange & Act
        var property = RuntimeElement.LabelProperty;

        // Assert
        Assert.Equal(BindingMode.TwoWay, property.DefaultBindingMode);
    }

    // ------------------------------------------------------------
    // Callback
    // ------------------------------------------------------------

    [Fact]
    public void PropertyChangedCallbackIsInvoked()
    {
        // Arrange
        var element = CreateElement();

        // Act
        element.Scale = 3d;

        // Assert
        Assert.Equal(1, element.ChangedCount);
        Assert.Equal(1d, element.OldValue);
        Assert.Equal(3d, element.NewValue);
    }

    [Fact]
    public void PropertyChangingCallbackIsInvoked()
    {
        // Arrange
        var element = CreateElement();

        // Act
        element.Scale = 3d;

        // Assert
        Assert.Equal(1, element.ChangingCount);
    }

    [Fact]
    public void CoerceCallbackIsApplied()
    {
        // Arrange
        var element = CreateElement();

        // Act
        element.Scale = 100d;

        // Assert
        Assert.Equal(10d, element.Scale);
    }

    [Fact]
    public void ValidateCallbackRejectsInvalidValue()
    {
        // Arrange
        var element = CreateElement();
        element.Label = "abc";

        // Act
        // A value rejected by validateValue is ignored, and the previous value is kept
        element.Label = "too long value";

        // Assert
        Assert.Equal("abc", element.Label);
    }
}

internal sealed partial class RuntimeElement : BindableObject
{
    [BindableProperty(DefaultValue = 1d, PropertyChanged = nameof(OnScaleChanged), PropertyChanging = nameof(OnScaleChanging), Coerce = nameof(CoerceScale))]
    public partial double Scale { get; set; }

    [BindableProperty(DefaultBindingMode = BindingMode.TwoWay, Validate = nameof(ValidateLabel))]
    public partial string? Label { get; set; }

    public double MaximumScale { get; set; } = 10d;

    public int MaximumLabelLength { get; set; } = 5;

    public int ChangedCount { get; private set; }

    public int ChangingCount { get; private set; }

    public double ChangingDelta { get; private set; }

    public double OldValue { get; private set; }

    public double NewValue { get; private set; }

    private void OnScaleChanged(double oldValue, double newValue)
    {
        ChangedCount++;
        OldValue = oldValue;
        NewValue = newValue;
    }

    private void OnScaleChanging(double oldValue, double newValue)
    {
        ChangingCount++;
        ChangingDelta = newValue - oldValue;
    }

    private double CoerceScale(double value) => Math.Clamp(value, 0d, MaximumScale);

    private bool ValidateLabel(string? value) => value is null || (value.Length <= MaximumLabelLength);
}
