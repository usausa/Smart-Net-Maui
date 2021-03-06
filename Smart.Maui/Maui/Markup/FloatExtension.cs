namespace Smart.Maui.Markup;

[ContentProperty("Value")]
public sealed class FloatExtension : IMarkupExtension
{
    public float Value { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider) => Value;
}
