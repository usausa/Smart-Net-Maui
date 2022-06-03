namespace Smart.Maui.Resolver;

public sealed class ResolveExtension : IMarkupExtension
{
    public Type Type { get; set; } = default!;

    public object? ProvideValue(IServiceProvider serviceProvider) => ResolveProvider.Default.Resolve(Type);
}
