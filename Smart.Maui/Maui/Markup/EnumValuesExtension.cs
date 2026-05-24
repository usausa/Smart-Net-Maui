namespace Smart.Maui.Markup;

using System.Diagnostics.CodeAnalysis;

[AcceptEmptyServiceProvider]
[ContentProperty("Type")]
[RequiresDynamicCode("Enum.GetValues uses MakeGenericType internally and may not work in AOT scenarios.")]
public sealed class EnumValuesExtension : IMarkupExtension
{
    public Type Type { get; set; }

    public EnumValuesExtension(Type type)
    {
        Type = type;
    }

    public object ProvideValue(IServiceProvider serviceProvider) => Enum.GetValues(Type);
}
