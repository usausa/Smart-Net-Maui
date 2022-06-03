namespace Smart.Maui.Data;

using System.Globalization;

public sealed class ObjectToBoolConverter<T> : IValueConverter
{
    public T TrueValue { get; set; } = default!;

    public T FalseValue { get; set; } = default!;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is T typedValue && Equals(typedValue, TrueValue);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is true ? TrueValue : FalseValue;
    }
}
