namespace Smart.Maui.Markup;

[AcceptEmptyServiceProvider]
[ContentProperty("Value")]
public sealed class Int64Extension : IMarkupExtension
{
    public long Value { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider) => Value;
}
