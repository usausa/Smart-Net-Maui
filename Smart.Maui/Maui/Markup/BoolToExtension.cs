namespace Smart.Maui.Markup;

using Smart.Maui.Data;

public sealed class BoolToTextExtension : IMarkupExtension<BoolToTextConverter>
{
    public string? True { get; set; }

    public string? False { get; set; }

    public BoolToTextConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { TrueValue = True, FalseValue = False };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}

public sealed class BoolToColorExtension : IMarkupExtension<BoolToColorConverter>
{
    public Color True { get; set; } = Colors.Transparent;

    public Color False { get; set; } = Colors.Transparent;

    public BoolToColorConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { TrueValue = True, FalseValue = False };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
