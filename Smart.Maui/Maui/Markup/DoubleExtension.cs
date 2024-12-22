namespace Smart.Maui.Markup;

[AcceptEmptyServiceProvider]
[ContentProperty("Value")]
public sealed class DoubleExtension : IMarkupExtension
{
    public double Value { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider) => Value;
}
