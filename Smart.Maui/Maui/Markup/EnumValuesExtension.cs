namespace Smart.Maui.Markup;

using System.Diagnostics.CodeAnalysis;

[AcceptEmptyServiceProvider]
[ContentProperty("Type")]
public sealed class EnumValuesExtension : IMarkupExtension
{
    public Type Type { get; set; }

    public EnumValuesExtension(Type type)
    {
        Type = type;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2111", Justification = "Enum type is specified by user; they must ensure it is preserved")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Enum.GetValues(Type) uses dynamic code; use Enum.GetValues<TEnum>() for AOT-safe usage")]
    public object ProvideValue(IServiceProvider serviceProvider) => Enum.GetValues(Type);
}
