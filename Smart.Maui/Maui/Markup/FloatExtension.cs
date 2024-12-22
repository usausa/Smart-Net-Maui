namespace Smart.Maui.Markup;

[AcceptEmptyServiceProvider]
[ContentProperty("Value")]
public sealed class FloatExtension : IMarkupExtension
{
    public float Value { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider) => Value;
}
