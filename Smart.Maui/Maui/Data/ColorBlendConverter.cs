namespace Smart.Maui.Data;

using System.Globalization;

#pragma warning disable IDE0032
public sealed class ColorBlendConverter : IValueConverter
{
    private double raito;

    public Color Color { get; set; } = Colors.Transparent;

    public double Raito
    {
        get => raito;
        set
        {
            if ((value < 0d) || (value > 1d))
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            raito = value;
        }
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Color color)
        {
            return null;
        }

        var ratio = (float)raito;
        var r = (color.Red * (1f - ratio)) + (Color.Red * ratio);
        var g = (color.Green * (1f - ratio)) + (Color.Green * ratio);
        var b = (color.Blue * (1f - ratio)) + (Color.Blue * ratio);
        return new Color(r, g, b);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
