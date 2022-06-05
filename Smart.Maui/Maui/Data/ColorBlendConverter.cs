namespace Smart.Maui.Data;

using System.Globalization;

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

        var r = Math.Min((byte)Math.Round(color.Red + ((Color.Red - color.Red) * raito)), (byte)255);
        var g = Math.Min((byte)Math.Round(color.Green + ((Color.Green - color.Green) * raito)), (byte)255);
        var b = Math.Min((byte)Math.Round(color.Blue + ((Color.Blue - color.Blue) * raito)), (byte)255);
        return new Color(r, g, b);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
