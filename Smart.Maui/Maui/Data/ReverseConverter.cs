namespace Smart.Maui.Data;

using System.Globalization;

public sealed class ReverseConverter : IValueConverter
{
    private static readonly object BoxedTrue = true;
    private static readonly object BoxedFalse = false;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool boolValue ? (boolValue ? BoxedFalse : BoxedTrue) : value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool boolValue ? (boolValue ? BoxedFalse : BoxedTrue) : value;
    }
}
