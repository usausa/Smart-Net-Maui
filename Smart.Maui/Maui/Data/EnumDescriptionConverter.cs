namespace Smart.Maui.Data;

using System.ComponentModel;
using System.Globalization;
using System.Reflection;

public sealed class EnumDescriptionConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return null;
        }

        if (value is Enum enumValue)
        {
            var type = enumValue.GetType();
            var mis = type.GetMember(enumValue.ToString());
            if (mis.Length > 0)
            {
                var attr = mis[0].GetCustomAttribute<DescriptionAttribute>();
                if (attr is not null)
                {
                    return attr.Description;
                }
            }
        }

        return value.ToString();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
