namespace Smart.Maui.Markup;

[ContentProperty("Type")]
public sealed class EnumValuesExtension : IMarkupExtension
{
    public Type Type { get; set; }

    public EnumValuesExtension(Type type)
    {
        Type = type;
    }

    public object ProvideValue(IServiceProvider serviceProvider) => Enum.GetValues(Type);
}
