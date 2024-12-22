namespace Smart.Maui.Resolver;

[AcceptEmptyServiceProvider]
public sealed class ResolveExtension : IMarkupExtension
{
    public Type Type { get; set; } = default!;

    public object? ProvideValue(IServiceProvider serviceProvider) => ResolveProvider.Default.GetService(Type);
}
