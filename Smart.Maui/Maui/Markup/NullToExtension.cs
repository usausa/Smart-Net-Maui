namespace Smart.Maui.Markup;

using Smart.Maui.Data;

public sealed class NullToBoolExtension : IMarkupExtension<NullToBoolConverter>
{
    public bool Invert { get; set; }

    public bool HandleEmptyString { get; set; }

    public NullToBoolConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { NullValue = !Invert, NonNullValue = Invert, HandleEmptyString = HandleEmptyString };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}

public sealed class NullToTextExtension : IMarkupExtension<NullToTextConverter>
{
    public string? Null { get; set; }

    public string? NonNull { get; set; }

    public bool HandleEmptyString { get; set; }

    public NullToTextConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { NullValue = Null, NonNullValue = NonNull, HandleEmptyString = HandleEmptyString };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}

public sealed class NullToColorExtension : IMarkupExtension<NullToColorConverter>
{
    public Color Null { get; set; } = Colors.Transparent;

    public Color NonNull { get; set; } = Colors.Transparent;

    public bool HandleEmptyString { get; set; }

    public NullToColorConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { NullValue = Null, NonNullValue = NonNull, HandleEmptyString = HandleEmptyString };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
