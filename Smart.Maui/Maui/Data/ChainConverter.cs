namespace Smart.Maui.Data;

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

[ContentProperty("Converters")]
public sealed class ChainConverter : IValueConverter
{
    // ReSharper disable once CollectionNeverUpdated.Global
    public Collection<IValueConverter> Converters { get; } = [];

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var result = value;
        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = 0; i < Converters.Count; i++)
        {
            result = Converters[i].Convert(result, targetType, parameter, culture);
            if (IsSentinel(result))
            {
                break;
            }
        }

        return result;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var result = value;
        for (var i = Converters.Count - 1; i >= 0; i--)
        {
            result = Converters[i].ConvertBack(result, targetType, parameter, culture);
            if (IsSentinel(result))
            {
                break;
            }
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Binding.DoNothing is a sentinel object and reading it does not use string path bindings.")]
    private static bool IsSentinel(object? value) =>
        (value == Binding.DoNothing) || (value == BindableProperty.UnsetValue);
}
