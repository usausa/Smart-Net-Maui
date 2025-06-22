namespace Smart.Maui.Data;

using System.Globalization;

using Smart.Linq;

public sealed class AllConverter : IMultiValueConverter
{
    public bool Invert { get; set; }

    public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        return values.All(culture, static (x, s) => System.Convert.ToBoolean(x, s)) ? !Invert : Invert;
    }

    public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
