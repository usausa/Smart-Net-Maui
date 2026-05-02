namespace Smart.Maui.Data;

using System.Globalization;

public sealed class ParameterEqualsConverter : IValueConverter
{
    private static readonly object BoxedTrue = true;
    private static readonly object BoxedFalse = false;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Equals(value, parameter) ? BoxedTrue : BoxedFalse;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Equals(value, true) ? parameter : null;
    }
}
