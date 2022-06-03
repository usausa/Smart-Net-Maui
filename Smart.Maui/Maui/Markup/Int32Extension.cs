namespace Smart.Maui.Markup;

[ContentProperty("Value")]
public sealed class Int32Extension : IMarkupExtension
{
    public int Value { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider) => Value;
}
