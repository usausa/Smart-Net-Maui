namespace Smart.Maui.Data;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

public sealed class ParameterEqualsConverter : IValueConverter
{
    private static readonly object BoxedTrue = true;
    private static readonly object BoxedFalse = false;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Equals(value, parameter) ? BoxedTrue : BoxedFalse;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Binding.DoNothing is a sentinel object and reading it does not use string path bindings.")]
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Equals(value, true) ? parameter : Binding.DoNothing;
    }
}
