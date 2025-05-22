namespace Smart.Maui;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder ConfigureService(this MauiAppBuilder builder, Action<IServiceCollection> action)
    {
        action(builder.Services);
        return builder;
    }

    public static MauiAppBuilder UseMauiServices(this MauiAppBuilder builder)
    {
        builder.Services.AddMauiServices();
        return builder;
    }
}
