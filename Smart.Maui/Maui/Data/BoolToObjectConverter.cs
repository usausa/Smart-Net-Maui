namespace Smart.Maui.Data;

using System.Globalization;

public abstract class BoolToObjectConverter<T> : IValueConverter
{
    public T TrueValue { get; set; } = default!;

    public T FalseValue { get; set; } = default!;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? TrueValue : FalseValue;
        }

        return FalseValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is T typed)
        {
            if (typed is IEquatable<T> equatable)
            {
                if (equatable.Equals(TrueValue))
                {
                    return true;
                }
                if (equatable.Equals(FalseValue))
                {
                    return false;
                }
            }
            else
            {
                if (Equals(typed, TrueValue))
                {
                    return true;
                }
                if (Equals(typed, FalseValue))
                {
                    return false;
                }
            }
        }

        return Binding.DoNothing;
    }
}

public sealed class BoolToTextConverter : BoolToObjectConverter<string?>
{
}

public sealed class BoolToColorConverter : BoolToObjectConverter<Color>
{
    public BoolToColorConverter()
    {
        TrueValue = Colors.Transparent;
        FalseValue = Colors.Transparent;
    }
}
