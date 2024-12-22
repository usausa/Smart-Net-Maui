namespace Smart.Maui.Markup;

using Smart.Maui.Data;

[AcceptEmptyServiceProvider]
public sealed class ColorBlendExtension : IMarkupExtension<ColorBlendConverter>
{
    public Color Color { get; set; } = Colors.Transparent;

    public double Raito { get; set; }

    public ColorBlendConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { Color = Color, Raito = Raito };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
