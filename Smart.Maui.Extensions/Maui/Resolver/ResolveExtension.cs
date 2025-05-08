namespace Smart.Maui.Resolver;

using Smart.Mvvm.Resolver;

[AcceptEmptyServiceProvider]
public sealed class ResolveExtension : IMarkupExtension
{
    public Type Type { get; set; } = default!;

    public object? ProvideValue(IServiceProvider serviceProvider) => ResolveProvider.Default.GetService(Type);
}
