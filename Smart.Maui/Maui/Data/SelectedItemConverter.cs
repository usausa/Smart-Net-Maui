namespace Smart.Maui.Data;

using System.Globalization;

public sealed class SelectedItemConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is SelectedItemChangedEventArgs eventArgs ? eventArgs.SelectedItem : null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
