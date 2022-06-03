namespace Smart.Maui.Markup;

[ContentProperty("Value")]
public sealed class Int16Extension : IMarkupExtension
{
    public short Value { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider) => Value;
}
