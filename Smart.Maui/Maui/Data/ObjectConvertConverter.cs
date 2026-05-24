namespace Smart.Maui.Data;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Smart.Converter;

[RequiresDynamicCode("IObjectConverter.Convert uses MakeGenericType/MakeGenericMethod at runtime.")]
public sealed class ObjectConvertConverter : IValueConverter
{
    public IObjectConverter Converter { get; set; } = ObjectConverter.Default;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Converter.Convert(value, targetType);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Converter.Convert(value, targetType);
    }
}
