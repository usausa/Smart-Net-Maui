namespace Smart.Maui.Resolver;

using System.Diagnostics.CodeAnalysis;

[AcceptEmptyServiceProvider]
public sealed class ResolveExtension : IMarkupExtension
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public Type Type { get; set; } = default!;

    public object ProvideValue(IServiceProvider serviceProvider) => ResolveHelper.Resolve(Type);
}
