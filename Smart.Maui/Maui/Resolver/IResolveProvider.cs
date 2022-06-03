namespace Smart.Maui.Resolver;

public interface IResolveProvider
{
    object? Resolve(Type type);
}
