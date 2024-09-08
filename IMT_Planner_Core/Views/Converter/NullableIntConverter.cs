using System.Globalization;
using System.Windows.Data;

namespace IMT_Planner.Views.Converter;

public class NullableIntConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is int?)
        {
            int? nullableInt = (int?)value;
            return nullableInt?.ToString() ?? string.Empty;
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (string.IsNullOrWhiteSpace(value as string))
        {
            return null;
        }

        if (int.TryParse(value as string, out int result))
        {
            return result;
        }

        return null; // Or throw exception based on your requirement
    }
}