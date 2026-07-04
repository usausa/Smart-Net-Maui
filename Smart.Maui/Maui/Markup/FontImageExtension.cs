namespace Smart.Maui.Markup;

[AcceptEmptyServiceProvider]
[ContentProperty("Glyph")]
public sealed class FontImageExtension : IMarkupExtension
{
    public string? Glyph { get; set; }

    public string? FontFamily { get; set; }

    public Color? Color { get; set; }

    public double Size { get; set; } = 30d;

    public object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (Glyph is null)
        {
            return null;
        }

        var source = new FontImageSource
        {
            Glyph = Glyph,
            Size = Size
        };
        if (FontFamily is not null)
        {
            source.FontFamily = FontFamily;
        }

        if (Color is not null)
        {
            source.Color = Color;
        }

        return source;
    }
}
