namespace Smart.Maui.Markup;

using Smart.Maui.Data;

[AcceptEmptyServiceProvider]
public sealed class TextToBoolExtension : IMarkupExtension<TextToBoolConverter>
{
    public string? True { get; set; }

    public string? False { get; set; }

    public TextToBoolConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { TrueValue = True, FalseValue = False };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}

[AcceptEmptyServiceProvider]
public sealed class IntToBoolExtension : IMarkupExtension<IntToBoolConverter>
{
    public int True { get; set; }

    public int False { get; set; }

    public IntToBoolConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { TrueValue = True, FalseValue = False };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
