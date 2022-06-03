namespace Smart.Maui.Resolver;

using Smart.Resolver;

public sealed class SmartResolveProvider : IResolveProvider
{
    private readonly SmartResolver resolver;

    public SmartResolveProvider(SmartResolver resolver)
    {
        this.resolver = resolver;
    }

    public object Resolve(Type type) => resolver.Get(type);
}
