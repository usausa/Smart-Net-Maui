namespace Smart.Maui.Markup;

using Smart.Maui.Data;

[AcceptEmptyServiceProvider]
public sealed class ContainsToBoolExtension : IMarkupExtension<ContainsToBoolConverter>
{
    public bool Invert { get; set; }

    public ContainsToBoolConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { TrueValue = !Invert, FalseValue = Invert };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}

[AcceptEmptyServiceProvider]
public sealed class ContainsToTextExtension : IMarkupExtension<ContainsToTextConverter>
{
    public string True { get; set; } = string.Empty;

    public string False { get; set; } = string.Empty;

    public ContainsToTextConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { TrueValue = True, FalseValue = False };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}

[AcceptEmptyServiceProvider]
public sealed class ContainsToColorExtension : IMarkupExtension<ContainsToColorConverter>
{
    public Color True { get; set; } = Colors.Transparent;

    public Color False { get; set; } = Colors.Transparent;

    public ContainsToColorConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { TrueValue = True, FalseValue = False };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
