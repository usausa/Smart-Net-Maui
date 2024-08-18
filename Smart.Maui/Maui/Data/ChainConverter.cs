namespace Smart.Maui.Data;

using System.Collections.ObjectModel;
using System.Globalization;

[ContentProperty("Converters")]
public sealed class ChainConverter : IValueConverter
{
    public Collection<IValueConverter> Converters { get; } = new([]);

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Converters.Aggregate(value, (current, converter) =>
            converter.Convert(current, targetType, parameter, culture));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Converters.Reverse().Aggregate(value, (current, converter) =>
            converter.ConvertBack(current, targetType, parameter, culture));
    }
}
