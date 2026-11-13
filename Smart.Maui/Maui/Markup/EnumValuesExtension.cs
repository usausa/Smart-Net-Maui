namespace Smart.Maui.Markup;

[AcceptEmptyServiceProvider]
[ContentProperty("Type")]
public sealed class EnumValuesExtension : IMarkupExtension
{
    public Type Type { get; set; }

    public EnumValuesExtension(Type type)
    {
        Type = type;
    }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        var values = Enum.GetValuesAsUnderlyingType(Type);
        var result = new object[values.Length];
        for (var i = 0; i < values.Length; i++)
        {
            result[i] = Enum.ToObject(Type, values.GetValue(i)!);
        }

        return result;
    }
}
