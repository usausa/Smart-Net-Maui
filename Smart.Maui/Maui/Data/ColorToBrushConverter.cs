namespace Smart.Maui.Data;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

public sealed class ColorToBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is Color color ? new SolidColorBrush(color) : null;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Binding.DoNothing is a sentinel object and reading it does not use string path bindings.")]
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is SolidColorBrush brush ? brush.Color : Binding.DoNothing;
    }
}
