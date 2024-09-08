using System.Globalization;
using System.Windows.Data;

namespace IMT_Planner.Views.Converter;

public class NullToFalseConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value != null && (bool)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}