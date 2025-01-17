namespace Smart.Maui.Markup;

using System.Text.RegularExpressions;

using Smart.Maui.Data;

[AcceptEmptyServiceProvider]
public sealed class TextReplaceExtension : IMarkupExtension<TextReplaceConverter>
{
    public string Pattern { get; set; } = string.Empty;

    public string Replacement { get; set; } = string.Empty;

    public RegexOptions Options { get; set; }

    public bool ReplaceAll { get; set; } = true;

    public TextReplaceConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { Pattern = Pattern, Replacement = Replacement, Options = Options, ReplaceAll = ReplaceAll };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
