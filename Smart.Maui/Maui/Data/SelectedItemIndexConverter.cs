namespace Smart.Maui.Data;

using System.Globalization;

public sealed class SelectedItemIndexConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is SelectedItemChangedEventArgs eventArgs ? eventArgs.SelectedItemIndex : -1;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
