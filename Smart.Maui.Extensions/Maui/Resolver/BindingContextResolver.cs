namespace Smart.Maui.Resolver;

public static class BindingContextResolver
{
    public static readonly BindableProperty TypeProperty = BindableProperty.CreateAttached(
        "Type",
        typeof(Type),
        typeof(BindingContextResolver),
        null,
        propertyChanged: HandleTypePropertyChanged);

    private static readonly BindableProperty ResolvedProperty = BindableProperty.CreateAttached(
        "Resolved",
        typeof(object),
        typeof(BindingContextResolver),
        null);

    public static readonly BindableProperty DisposeOnChangedProperty = BindableProperty.CreateAttached(
        "DisposeOnChanged",
        typeof(bool),
        typeof(BindingContextResolver),
        true);

    public static Type GetType(BindableObject obj) =>
        (Type)obj.GetValue(TypeProperty);

    public static void SetType(BindableObject obj, Type value) =>
        obj.SetValue(TypeProperty, value);

    public static bool GetDisposeOnChanged(BindableObject obj) =>
        (bool)obj.GetValue(DisposeOnChangedProperty);

    public static void SetDisposeOnChanged(BindableObject obj, bool value) =>
        obj.SetValue(DisposeOnChangedProperty, value);

    private static void HandleTypePropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        var context = newValue is not null ? ResolveHelper.Resolve((Type)newValue) : null;

        var resolved = bindable.GetValue(ResolvedProperty);
        bindable.SetValue(ResolvedProperty, context);
        bindable.BindingContext = context;

        if (GetDisposeOnChanged(bindable) &&
            !ReferenceEquals(resolved, context) &&
            (resolved is IDisposable disposable))
        {
            disposable.Dispose();
        }
    }
}
