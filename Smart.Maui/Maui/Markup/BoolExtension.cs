namespace Smart.Maui.Markup;

[AcceptEmptyServiceProvider]
[ContentProperty("Value")]
public sealed class BoolExtension : IMarkupExtension
{
    public bool Value { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider) => Value;
}
